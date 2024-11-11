using System.Collections.Immutable;
using System.Xml.Linq;
using XEducation.Expressions;
using XEducation.Modules.Biz.Deposits;
using XEducation.Ui;

namespace XEducation.Modules.Maths.Tales;

internal class TalesTask:DepositCalculationTaskBase
{
    public static ImmutableArray<TalesTask> Create(int count)
    {
        var l = new List<TalesTask>(count);
        for (var i = 0; i < count; i++)
            l.Add(GenerateRandom());
        return [..l];
    }

    private static TalesTask GenerateRandom()
    {
        while (true)
        {
            var t = new TalesTask
            {
                Model = TalesModelGenerator.Create()
            };
            t.UpdateTaskFromModel();
            if (t.Accept())
                return t;
            Console.WriteLine("Rejected");
        }
    }

    private bool Accept()
    {
        var q = X1 / X2;
        if (q is not IntegerExpression f)
            return false;
        if (f.Value != 1)
            return false;
        return AcceptNumber(X1);

        bool AcceptNumber(NumericExpression n)
        {
            switch (n)
            {
                case FractionExpression f:
                {
                    if (f.Numerator is IntegerExpression ie)
                    {
                        return Math.Abs(ie.Value) < 1000;
                    }
                    return false;
                }
                case IntegerExpression ie2:
                    return Math.Abs(ie2.Value) < 1000;
                default:
                    return true;
            }
        }
    }

    public void FlushTask(FlowDocumentHelper helper)
    {
        helper.AddParagraphX(GetStartingText(false));
        helper.AddDrawing(TalesDrawer.DrawToCanvas(Model));
    }
    
    public void FlushTaskSolution(FlowDocumentHelper helper)
    {
        helper.AddParagraphX(GetStartingText(true));
        var unknown = Items[GuessIndex];

        helper.AddParagraphX("Szukana długość odcinka ",
            Bold(unknown.Name),
            " to:",
            Bold(unknown.Len));

        helper.AddDrawing(TalesDrawer.DrawToCanvas(Model));
    }

    private IEnumerable<object> GetStartingText(bool isSolution)
    {
        var unknown = Items[GuessIndex];
        if (!isSolution)
            yield return "Dla poniższego rysunku wylicz długość odcinka " + unknown.Name;
        foreach (var i in Items)
        {
            if (i==unknown && !isSolution) continue;
            if (isSolution) 
                yield return " " + i;
            else
            {
                yield return new XElement("br");
                yield return "• " + i;
            }
        }

        /*yield return new XElement("br");
        yield return "- " + X1;
        yield return new XElement("br");
        yield return "- " + X2;*/
    }

    private void UpdateTaskFromModel()
    {
        var combinations = Model.GetCombinations();

        int idx1, idx2;
        do
        {
            idx1 = RandomHelper.RangeIncludingInt(0, combinations.Count - 1);
            idx2 = RandomHelper.RangeIncludingInt(0, combinations.Count - 1);
        } while (idx1 == idx2);

        var c1 = combinations[idx1];
        var c2 = combinations[idx2];

        var s1 = c1.TryGetSingle();
        var s2 = c2.TryGetSingle();

        var a = c1.GetX(true, Model.A);
        var b = c1.GetX(false, Model.B);

        var c = c2.GetX(true, Model.A);
        var d = c2.GetX(false, Model.B);

        if (s1 is not null && s2 is not null)
        {
            var use1 = Random.Shared.NextDouble() < 1.75;
            if (use1)
            {
                b = s1.ToPairItem();
                d = s2.ToPairItem();
            }
        }

        Pair1 = new TalesModel.Pair(a, b);
        Pair2 = new TalesModel.Pair(c, d);
        Items      = [a, b, c, d];
        GuessIndex = RandomHelper.RangeIncludingInt(0, 3);

        var letters = Items.SelectMany(a => new[] { a.P1, a.P2 }).ToHashSet();
        var newLines = this.Model.Lines.Where(a => letters.Contains(a.P1) && letters.Contains(a.P2)).ToImmutableArray();
        if (newLines.Length != Model.Lines.Length)
        {
            Model = new TalesModel(newLines, Model.RotationAngleDeg, Model.A, Model.B);
        }
        
    }

    private NumericExpression X2 => Pair1.B.Len * Pair2.A.Len;

    private NumericExpression X1 => Pair1.A.Len * Pair2.B.Len;

    public TalesModel Model { get; set; }

    public int                   GuessIndex { get; private set; }
    public TalesModel.PairItem[] Items      { get; private set; }
    public TalesModel.Pair       Pair1      { get; private set; }
    public TalesModel.Pair       Pair2      { get; private set; }
}