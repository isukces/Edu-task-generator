namespace XEducation.Expressions;

public abstract class NumericExpression : IExpression
{
    public NumericExpression Abs()
    {
        if (this.GetValue() < 0)
            return Minus();
        return this;

    }

    public abstract NumericExpression Minus();

    public static NumericExpression operator *(NumericExpression a, NumericExpression b)
    {
        var f1 = a.TryConvertToFraction();
        var f2 = b.TryConvertToFraction();
        var f  = f1 * f2;
        return f.ToExpression();
    }
    public static NumericExpression operator /(NumericExpression a, NumericExpression b)
    {
        var f1 = a.TryConvertToFraction();
        var f2 = b.TryConvertToFraction();
        var f  = f1 / f2;
        return f.ToExpression();
    }
    
    public static NumericExpression operator -(NumericExpression a, NumericExpression b)
    {
        var f1 = a.TryConvertToFraction();
        var f2 = b.TryConvertToFraction();
        var f  = f1 - f2;
        return f.ToExpression();
    }

    public abstract decimal    GetValue();
    public abstract XFraction? TryConvertToFraction();
}