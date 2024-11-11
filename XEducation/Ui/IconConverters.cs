using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using MahApps.Metro.IconPacks;

namespace XEducation.Ui;


/// <summary>
/// Converts a <see cref="PackIcon{TKind}" /> to an ImageSource.
/// Use the ConverterParameter to pass a Brush.
/// </summary>
public class PackIconImageSourceConverter : MarkupExtension, IValueConverter
{
    /// <summary>
    /// Gets or sets the thickness to draw the icon with.
    /// </summary>
    public double Thickness { get; set; } = 0.25;

    public static ImageSource? ConvertIconKind(object iconKind, Brush? brush)
    {
        var obj= new PackIconImageSourceConverter()
            .Convert(iconKind, typeof(ImageSource), brush, CultureInfo.InvariantCulture);
        return obj as ImageSource;
    }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
            return null;

        if (value is PackIconMaterialKind)
            return new PackIconMaterialImageSourceConverter { 
                           Thickness = Thickness 
                       }.Convert(value, targetType, parameter, culture);
        if (value is PackIconPhosphorIconsKind)
            return new PackIconPhosphorIconsImageSourceConverter { 
                           Thickness = Thickness 
                       }.Convert(value, targetType, parameter, culture);
        if (value is PackIconVaadinIconsKind)
            return new PackIconVaadinIconsImageSourceConverter { 
                           Thickness = Thickness 
                       }.Convert(value, targetType, parameter, culture);
        return null;
    }

    public object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}

 public class PackIconMaterialImageSourceConverter : PackIconImageSourceConverterBase<PackIconMaterialKind>
{
    protected override ImageSource? CreateImageSource(object value, Brush foregroundBrush, double penThickness)
    {
        var packIcon = new PackIconMaterial { Kind = (PackIconMaterialKind)value };

        var geometryDrawing = new GeometryDrawing
        {
            Geometry = Geometry.Parse(packIcon.Data),
            Brush = foregroundBrush,
            Pen = new Pen(foregroundBrush, penThickness)
        };

        var drawingGroup = new DrawingGroup { Children = { geometryDrawing } };

        return new DrawingImage { Drawing = drawingGroup };
    }
}
public class PackIconPhosphorIconsImageSourceConverter : PackIconImageSourceConverterBase<PackIconPhosphorIconsKind>
{
    protected override ImageSource? CreateImageSource(object value, Brush foregroundBrush, double penThickness)
    {
        var packIcon = new PackIconPhosphorIcons { Kind = (PackIconPhosphorIconsKind)value };

        var geometryDrawing = new GeometryDrawing
        {
            Geometry = Geometry.Parse(packIcon.Data),
            Brush = foregroundBrush,
            Pen = new Pen(foregroundBrush, penThickness)
        };

        var drawingGroup = new DrawingGroup { Children = { geometryDrawing } };

        return new DrawingImage { Drawing = drawingGroup };
    }
}
public class PackIconVaadinIconsImageSourceConverter : PackIconImageSourceConverterBase<PackIconVaadinIconsKind>
{
    protected override ImageSource? CreateImageSource(object value, Brush foregroundBrush, double penThickness)
    {
        var packIcon = new PackIconVaadinIcons { Kind = (PackIconVaadinIconsKind)value };

        var geometryDrawing = new GeometryDrawing
        {
            Geometry = Geometry.Parse(packIcon.Data),
            Brush = foregroundBrush,
            Pen = new Pen(foregroundBrush, penThickness)
        };

        var drawingGroup = new DrawingGroup { Children = { geometryDrawing } };

        return new DrawingImage { Drawing = drawingGroup };
    }
}
