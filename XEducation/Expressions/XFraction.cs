namespace XEducation.Expressions;

public class XFraction
{
    public override string ToString()
    {
        return $"{Numerator}/{Denominator}";
    }

    public XFraction(long numerator, long denominator)
    {
        var div = FindCommonDivisor(numerator, denominator);
        Numerator   = numerator / div;
        Denominator = denominator / div;
    }

    private static long FindCommonDivisor(long a, long b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);
        if (a == b || a == 0) return b;
        if (a < b)
            (a, b) = (b, a);
        while (b != 0)
        {
            var t = b;
            b = a % b;
            a = t;
        }

        return a;
    }

    public static XFraction operator *(XFraction a, XFraction b)
    {
        return new XFraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
    }

    public static XFraction operator -(XFraction a, XFraction b)
    {
        var n = a.Numerator * b.Denominator - b.Numerator * a.Denominator;
        var d = a.Denominator * b.Denominator;
        return new XFraction(n, d);
    }

    public static XFraction operator /(XFraction a, XFraction b)
    {
        return new XFraction(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
    }

    public long Numerator   { get; }
    public long Denominator { get; }

    public NumericExpression ToExpression()
    {
        if (Denominator == 1)
            return new IntegerExpression((int)Numerator);
        return new FractionExpression(new IntegerExpression((int)Numerator), new IntegerExpression((int)Denominator));
    }
}