namespace XEducation.Expressions;

public class DecimalExpression : NumericExpression
{
    public override string ToString() => Value.ToString();

    public DecimalExpression(decimal value)
    {
        Value = value;
    }

    public override NumericExpression Minus()
    {
        return new DecimalExpression(-Value);
    }

    public override decimal GetValue()
    {
        return Value;
    }

    public override XFraction? TryConvertToFraction()
    {
        var denominator = 1;
        int numerator   = 0;
        while (denominator < 1_000_000_000)
        {
            decimal value = Value * denominator;
            decimal m     = (int)Math.Round(value);
            numerator = (int)m;
            if (value == m)
                break;
            denominator *= 10;
        }

        // var numerator = (int)Value * denominator + whole;
        return new(numerator, denominator);
    }

    public decimal Value { get; }
}