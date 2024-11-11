namespace XEducation.Common;

public class Triangle(double a, double b, double c)
{
    private static bool CheckPair(double a, double b, double c)
    {
        return a + b > c;
    }

    private static bool CheckPair2(double a, double b, double c)
    {
        return CheckPair(a, b, c)
               && CheckPair(b, c, a)
               && CheckPair(c, a, b);
    }

    public static Triangle? TryMake(double a, double b, double c)
    {
        if (CheckPair2(a, b, c))
            return new Triangle(a, b, c);
        return null;
    }

    public double A { get; } = a;
    public double B { get; } = b;
    public double C { get; } = c;

    public double CosAb => GetCos(A, B, C);
    public double CosAc => GetCos(A, C, B);
    public double CosBc => GetCos(B, C, A);


    public double AngleAb => Math.Acos(GetCos(A, B, C)) * (180 / Math.PI);
    public double AngleAc => Math.Acos(GetCos(A, C, B)) * (180 / Math.PI);
    public double AngleBc => Math.Acos(GetCos(B, C, A)) * (180 / Math.PI);



    public static double GetCos(double a, double b, double c)
    {
        return (a * a + b * b - c * c) / (2 * a * b);
    }
}
