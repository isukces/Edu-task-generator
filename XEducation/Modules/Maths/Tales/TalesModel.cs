using System.Collections.Immutable;
using XEducation.Expressions;

namespace XEducation.Modules.Maths.Tales;

internal class TalesModel
{
    public TalesModel(ImmutableArray<TalesLine> lines, double rotationAngleDeg,
        NumericExpression a, NumericExpression b)
    {
        Lines            = lines;
        RotationAngleDeg = rotationAngleDeg;
        A                = a;
        B                = b;
    }

    public IReadOnlyList<PointInfo> GetCombinations()
    {
        var l = new Dictionary<NumericExpression, TalesLine?>();
        l[new IntegerExpression(0)] = null;
        foreach (var line in Lines) l[line.Location] = line;

        var data = l.OrderBy(a => a.Key.GetValue()).ToArray();

        var result = new List<PointInfo>();
        for (var i = 0; i < data.Length; i++)
        for (var j = i + 1; j < data.Length; j++)
            result.Add(new PointInfo(data[i].Value, data[j].Value));
        return result;
    }


    public ImmutableArray<TalesLine> Lines            { get; }
    public double                    RotationAngleDeg { get; }
    public NumericExpression         A                { get; }
    public NumericExpression         B                { get; }


    public record Pair(PairItem A, PairItem B);

    public record PairItem(NumericExpression Len, string P1, string P2)
    {
        public override string ToString()
        {
            return $"odcinek {Name} = {Len}";
        }

        public string Name
        {
            get
            {
                if (P2 == "O") return P2 + P1;
                if (P1 == "O") return P1 + P2;
                if (StringComparer.OrdinalIgnoreCase.Compare(P1, P2) < 0) return P1 + P2;
                return P2 + P1;
            }
        }
    }


    public record ITypeInfo2;

    public record PointInfo(TalesLine? A, TalesLine? B)
    {
        public IEnumerable<ITypeInfo2> GetGroup1()
        {
            yield return new ITypeInfo2();
        }

        public IEnumerable<ITypeInfo2> GetGroup2()
        {
            yield return new ITypeInfo2();
        }

        public PairItem GetX(bool first, NumericExpression factor)
        {
            var a = Make(A);
            var b = Make(B);
            var v = a.Item1 - b.Item1;
            if (v.GetValue() < 0)
                v = b.Item1 - a.Item1;

            return new PairItem(v, a.Item2, b.Item2);

            (NumericExpression, string) Make(TalesLine? line)
            {
                if (line is null)
                    return (new IntegerExpression(0), "O");
                return (line.Location * factor, first ? line.P1 : line.P2);
            }
        }

        public TalesLine? TryGetSingle()
        {
            if (A is null && B is null)
                throw new InvalidOperationException();
            if (A is null) return B;
            if (B is null) return A;
            return null;
        }
    }

    public class TalesLine(NumericExpression location, string p1, string p2)
    {
        public override string ToString()
        {
            return $"{Location} {P1} {P2}";
        }

        public TalesLine WithPoints(string p1, string p2)
        {
            return new TalesLine(Location, p1, p2);
        }

        public NumericExpression Location { get; } = location;
        public string            P1       { get; } = p1;
        public string            P2       { get; } = p2;

        
        public PairItem ToPairItem()
        {
            return new PairItem(Location.Abs() , P1, P2);
        }
    
    }
}