namespace XEducation.Expressions;

public class FractionExpression : NumericExpression
{
    public FractionExpression(int numerator, int denominator)
    {
        var commonDivisor = FindCommonDivisor(numerator, denominator);
        Numerator   = new IntegerExpression(numerator/commonDivisor);
        Denominator = new IntegerExpression(denominator/commonDivisor);
    }

    private static int FindCommonDivisor(int a, int b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);
        if (a == b || a ==0) return b;
        if (a<b) 
            (a, b) = (b, a);
        while (b != 0)
        {
            var t = b;
            b = a % b;
            a = t;
        }

        return a;
    }

    public override string ToString() => $"{Numerator}/{Denominator}";
    public FractionExpression(NumericExpression numerator, NumericExpression denominator)
    {
        Numerator   = numerator;
        Denominator = denominator;
    }

    public override NumericExpression Minus()
    {
        if (Denominator.GetValue() < 0)
            return new FractionExpression(Numerator, Denominator.Minus());
        return new FractionExpression(Numerator.Minus(), Denominator);
    }

    public override decimal GetValue()
    {
        return Numerator.GetValue() / Denominator.GetValue();
    }

    public override XFraction? TryConvertToFraction()
    {
        var numerator   = Numerator.TryConvertToFraction();
        var denominator = Denominator.TryConvertToFraction();
        if (numerator == null || denominator == null) return null;
        return numerator / denominator;
    }

    public NumericExpression Numerator   { get; }
    public NumericExpression Denominator { get; }
}