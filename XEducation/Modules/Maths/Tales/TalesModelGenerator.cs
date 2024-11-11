using iSukces.Mathematics;
using XEducation.Common;
using XEducation.Expressions;

namespace XEducation.Modules.Maths.Tales;

internal class TalesModelGenerator
{
    public static TalesModel Create()
    {
        var generator = new TalesModelGenerator();
        while (true)
        {
            var model = TalesModel1();
            if (model is not null)
                return model;
        }

        TalesModel? TalesModel1()
        {
            var model  = generator.CreateInternal();
            var l      = model.Lines.Select(q => (double)q.Location.GetValue()).OrderBy(a => a).ToArray();
            var minmax = l.GetMinMax();
            var amounts = new List<double>();
            for (var index = 1; index < l.Length; index++)
            {
                var x1     = l[index - 1];
                var x2     = l[index];
                var diff   = x2 - x1;
                var amount = diff / minmax.Length;
                if (amount < 0.25)
                    return null;
                amounts.Add(amount);
            }

            Console.WriteLine("Mina=" + amounts.Min());
            return model;
        }
    }

    private static void GenerateAb(out NumericExpression a, out NumericExpression b)
    {
        var options = new NumberGenerationOptions
        {
            MaxValue            = 40,
            MinValue            = 0,
            DecimalProbability  = 0,
            FractionProbability = 42,
            IntProbability      = 1,
            NegativeProbability = 0,
            ZeroProbability     = 0
        };

#if TEST_NUMBERS
        var hashSet = new HashSet<string>();
        for (var i = 0; i < 1000; i++)
        {
            a = RandomHelper.GenerateNumberExpression(options, CheckAb);
            hashSet.Add(a.ToString());
        }
        Console.WriteLine(hashSet.Count);
        Console.WriteLine(string.Join(", ", hashSet));
#endif

        do
        {
            a = RandomHelper.GenerateNumberExpression(options, CheckAb);
            b = RandomHelper.GenerateNumberExpression(options, CheckAb);
            var aValue = (double)a.GetValue();
            var bValue = (double)b.GetValue();

            var t = Triangle.TryMake(aValue, bValue, 1);
            if (t is null)
                continue;
            if (t.AngleAc < 20) continue;
            if (t.AngleBc < 20) continue;
            return;
        } while (true);


        static bool CheckAb(NumericExpression arg)
        {
            var value = (double)arg.GetValue();
            if (value < 1)
                value = 1 / value;
            if (value > 1.8)
                return false;

            if (arg is FractionExpression f)
            {
                if (f.Denominator.GetValue() == 1)
                    return false;
                if (f.Denominator is IntegerExpression ie)
                    if (ie.Value > 10)
                        return false;
            }

            return true;
        }
    }


    private TalesModel CreateInternal()
    {
        //var count = RandomHelper.RangeIncludingInt(2, 3);
        var count = 3;
        var lines1 = GenerateTriangles(count)
            .OrderBy(a => a.Location.GetValue())
            .ToArray();
        var letter = 'A';
        for (var i = 0; i < lines1.Length; i++)
        {
            var q = lines1[i];
            q         = q.WithPoints("" + letter++, "" + letter++);
            lines1[i] = q;
        }

        GenerateAb(out var a, out var b);
        return new TalesModel([..lines1], RandomHelper.RangeIncludingInt(0, 360), a, b);
    }

    private IEnumerable<TalesModel.TalesLine> GenerateTriangles(int count)
    {
        var options = new NumberGenerationOptions
        {
            MaxValue            = 100,
            MinValue            = 0,
            DecimalProbability  = 0,
            FractionProbability = 20,
            IntProbability      = 80,
            NegativeProbability = 0.5,
            ZeroProbability     = 0
        };


        for (var i = 0; i < count; i++)
        {
            var location = Get(options);
            yield return new TalesModel.TalesLine(location, "", "");
        }
    }

    private NumericExpression Get(NumberGenerationOptions options)
    {
        while (true)
        {
            var distance =
                RandomHelper.GenerateNumberExpression(options, q => Math.Abs(q.GetValue()) is > 10 and < 100);
            var isOk  = true;
            var value = distance.GetValue();
            foreach (var usedDistance in _usedDistances)
            {
                var howFar = Math.Abs(usedDistance - value);
                if (howFar > 1) continue;
                isOk = false;
                break;
            }

            if (!isOk) continue;
            _usedDistances.Add(value);
            return distance;
        }
    }

    #region Fields

    private readonly List<decimal> _usedDistances = new();

    #endregion
}