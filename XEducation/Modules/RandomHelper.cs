using XEducation.Expressions;

namespace XEducation.Modules;

public sealed class RandomHelper
{
    private RandomHelper()
    {
    }

    public static T FromOptions<T>(params T[] options)
    {
        return options[Random.Shared.Next(0, options.Length)];
    }

    public static NumericExpression GenerateNumberExpression(NumberGenerationOptions x,
        Func<NumericExpression, bool> accept)
    {
        while (true)
        {
            var e = GenerateNumberExpression(x);
            if (accept(e))
                return e;
        }
    }

    private static NumericExpression GenerateNumberExpression(NumberGenerationOptions x)
    {
        var isZero = GetRandomFlag(x.ZeroProbability);

        if (isZero)
            return new IntegerExpression(0);

        var isMinus = GetRandomFlag(x.NegativeProbability);

        var sum = x.FractionProbability + x.IntProbability + x.DecimalProbability;
        var r   = Random.Shared.Next(0, sum);
        if (r < x.FractionProbability)
            return GetFraction();
        if (r < x.FractionProbability + x.IntProbability)
            return GetInteger();
        return GetDecimal();


        FractionExpression GetFraction()
        {
            while (true)
            {
                var num = RangeIncludingInt(x.MinValue, x.MaxValue);
                var den = RangeIncludingInt(x.MinValue, x.MaxValue);
                if (den == 0 || num == 0 || num % den == 0) continue;
                if (isMinus)
                    num = -num;
                return new FractionExpression(num, den);
            }
        }

        NumericExpression GetInteger()
        {
            while (true)
            {
                var value = RangeIncludingInt(x.MinValue, x.MaxValue);
                if (value == 0) continue;
                if (isMinus)
                    value = -value;
                return new IntegerExpression(value);
            }
        }

        NumericExpression GetDecimal()
        {
            const int scale = 100;
            while (true)
            {
                var value = RangeIncludingInt(x.MinValue * scale, x.MaxValue * scale);
                if (value == 0 || value % scale == 0) continue;
                if (isMinus)
                    value = -value;
                return new DecimalExpression(value / (decimal)scale);
            }
        }
    }

    private static bool GetRandomFlag(double prob)
    {
        return prob > 0 && Random.Shared.NextDouble() < prob;
    }

    public static double RangeIncludingDouble(double min, double max, int round = -1)
    {
        var x = Random.Shared.NextDouble() * (max - min) + min;
        if (round >= 0)
            x = Math.Round(x, round);
        if (x < min)
            return min;
        if (x > max)
            return max;
        return x;
    }

    /*
    public static RandomHelper Instance => RandomHelperHolder.SingleIstance;

    private static class RandomHelperHolder
    {
        public static readonly RandomHelper SingleIstance = new RandomHelper();

    }
    */

    public static int RangeIncludingInt(int min, int max)
    {
        var x = Random.Shared.Next(min, max + 1);
        if (x < min || x > max)
            throw new InvalidOperationException();
        return x;
    }
}

public struct NumberGenerationOptions
{
    public int FractionProbability { get; set; }
    public int IntProbability      { get; set; }
    public int DecimalProbability  { get; set; }

    /// <summary>
    ///     0..1
    /// </summary>
    public double NegativeProbability { get; set; }

    /// <summary>
    ///     0..1
    /// </summary>
    public double ZeroProbability { get; set; }

    public int MaxValue { get; set; }
    public int MinValue { get; set; }
}