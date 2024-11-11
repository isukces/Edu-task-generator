using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using iSukces.Mathematics;
using XEducation.Common;

namespace XEducation.Modules.Maths.Tales;

internal class TalesDrawer
{
    private static void AddText(string n, Point p, Canvas canvas)
    {
        if (string.IsNullOrEmpty(n))
            return;
        var text = new TextBlock
        {
            Text       = n,
            Foreground = Brushes.Black,
            FontSize   = 20
        };
        Canvas.SetLeft(text, p.X - 10);
        Canvas.SetTop(text, p.Y - 10);
        canvas.Children.Add(text);
    }

    public static Canvas DrawToCanvas(TalesModel model)
    {
        var drawer = Make(model);
        return drawer.DrawTo();
    }

    private static TalesDrawer Make(TalesModel model)
    {
        var a = (double)model.A.GetValue();
        var b = (double)model.B.GetValue();
        var c = 1;
        var t = Triangle.TryMake(a, b, c);
        if (t is null)
            throw new InvalidOperationException();
        var cosTheta = t.CosAb;
        var sinTheta = Math.Sqrt(1 - cosTheta * cosTheta);
        if (double.IsNaN(sinTheta))
            sinTheta = 0;

        return new TalesDrawer
        {
            _rotation = SimpleCoordinateSystem2D.FromRotateAndTranslate(model.RotationAngleDeg, 0, 0),
            _a        = a,
            _b        = b,
            _cos      = cosTheta,
            _sin      = sinTheta,
            model     = model
        };
    }

    private void AppendLine(Line line, Canvas canvas, string a = "", string b = "")
    {
        //line = line.Extend(0.3);
        var path = new System.Windows.Shapes.Line
        {
            X1              = line.A.X,
            Y1              = line.A.Y,
            X2              = line.B.X,
            Y2              = line.B.Y,
            Stroke          = Brushes.Black,
            StrokeThickness = 2
        };
        canvas.Children.Add(path);
        line = line.Extend(-0.05);
        AddText(a, line.A, canvas);
        AddText(b, line.B, canvas);
    }

    private Canvas DrawTo()
    {
        const double xSize = 400;
        const double ySize = 400;

        pointSink.Add(new Point(0, 0));

        var lines = model.Lines.Select(l =>
        {
            var len = (double)l.Location.GetValue();
            return MapLine(len).Extend(0.1);
        }).ToArray();
        pointSink.AddRange(lines);

        var otherLines = GetTwoLines().Select(a => a.Extend(0.1)).ToArray();
        pointSink.AddRange(otherLines);


        var xScale = DoubleScale.MapCentered(pointSink.Select(point => point.X), 0, xSize);
        var yScale = DoubleScale.MapCentered(pointSink.Select(point => point.Y), 0, ySize);
        xScale.Scale = Math.Min(xScale.Scale, yScale.Scale) * 0.8;
        yScale.Scale = -xScale.Scale;
        yScale.Shrink();
        var canvas = new Canvas
        {
            Width  = xSize,
            Height = yScale.DstCenter * 2
        };


        for (var index = 0; index < lines.Length; index++)
        {
            var line     = lines[index];
            var triangle = model.Lines[index];
            AppendLine(ScaleLine(line), canvas, triangle.P1, triangle.P2);
        }

        foreach (var line in otherLines.Select(ScaleLine))
            AppendLine(line, canvas);

        AddText("O", ScalePoint(new Point()), canvas);
        return canvas;

        IEnumerable<Line> GetTwoLines()
        {
            var r   = model.Lines.Select(a => (double)a.Location.GetValue()).GetMinMax();
            var min = Math.Min(r.Min, 0);
            var max = Math.Max(r.Max, 0);
            var l1  = MapLine(min);
            var l2  = MapLine(max);
            yield return l1 with { B = l2.A };
            yield return l2 with { A = l1.B };
        }

        Line ScaleLine(Line a)
        {
            return new Line(ScalePoint(a.A), ScalePoint(a.B));
        }

        Point ScalePoint(Point p)
        {
            var x = xScale.Map(p.X);
            var y = yScale.Map(p.Y);
            return new Point(x, y);
        }
    }

    private Line MapLine(double len)
    {
        var bokA = len * _a;
        var bokB = len * _b;
        var p1   = new Point(bokA, 0);
        var p2   = new Point(bokB * _cos, bokB * _sin);
        p1 = _rotation.Transform(p1);
        p2 = _rotation.Transform(p2);
        return new Line(p1, p2);
    }

    #region Fields

    private          double                   _a;
    private          double                   _b;
    private          double                   _cos;
    private          SimpleCoordinateSystem2D _rotation = null!;
    private          double                   _sin;
    public           TalesModel               model;
    private readonly List<Point>              pointSink = [];

    #endregion
}