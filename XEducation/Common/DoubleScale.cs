using iSukces.Mathematics;

namespace XEducation.Common;

public class DoubleScale
{
    public static DoubleScale MapCentered(IEnumerable<double> values, double min, double max)
    {
        var range = values.GetMinMax();
        return new DoubleScale
        {
            SrcCenter = range.Center,
            DstCenter = (min + max) * 0.5,
            Scale     = (max - min) / range.Length,
            DataRange = range
        };
    }
    
    public double Map(double x)
    {
        return (x - SrcCenter) * Scale + DstCenter;
    }    
    public double Map1(double x)
    {
        return (x - SrcCenter) * Scale;
    }

    public double Scale { get; set; }

    public double DstCenter { get; set; }

    public double SrcCenter { get; set; }

    public MinMax DataRange { get; set; }

    public void Shrink()
    {
        var min = (DataRange.Min - SrcCenter) * Scale;
        var max = (DataRange.Max - SrcCenter) * Scale;
        DstCenter = Math.Abs(min - max) * 0.5;
        
    }
}