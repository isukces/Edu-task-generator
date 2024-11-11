using System.Windows;
using XEducation.Modules.Maths.Tales;

namespace XEducation.Common;

public static class Extensions
{
    public static bool IsNanOrInfinity(double x)
    {
        return double.IsNaN(x) ||double.IsInfinity(x);
    }

    public static void AddRange(this List<Point> sink, IEnumerable<Line> lines)
    {
        foreach (var line in lines)
        {
            sink.Add(line.A);
            sink.Add(line.B);
        }
    }
}