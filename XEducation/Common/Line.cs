using System.Windows;

namespace XEducation.Common;

public record Line(Point A, Point B)
{
    public Line Extend(double scale)
    {
        var v       = B - A;
        //var scale = d / v.Length;
        if (Extensions.IsNanOrInfinity(scale))
            return this;
        v *= scale;
        return new Line(A - v, B + v);
    }
}