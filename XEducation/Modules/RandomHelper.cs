namespace XEducation.Modules;

public sealed class RandomHelper
{
    private RandomHelper()
    {
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

    public static double RangeIncludingDouble(double min, double max, int round=-1)
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

    public static T FromOptions<T>(params T[] options)
    {
        return options[Random.Shared.Next(0, options.Length)];
    }
}
