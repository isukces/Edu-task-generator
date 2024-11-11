namespace XEducation.Expressions;

public class IntegerExpression : NumericExpression
{
    public IntegerExpression(long value)
    {
        Value = value;
    }

    public override NumericExpression Minus()
    {
        return new IntegerExpression(-Value);
    }

    public override decimal GetValue()
    {
        return Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public override XFraction? TryConvertToFraction()
    {
        return new XFraction(Value, 1);
    }

    public long Value { get; }
}