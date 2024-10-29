using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace XEducation.Ui;

/// <summary>
/// Converts a <see cref="PackIcon{TKind}" /> to an ImageSource.
/// Use the ConverterParameter to pass a Brush.
/// </summary>
public abstract class PackIconImageSourceConverterBase<TKind> : MarkupExtension, IValueConverter
{
    /// <summary>
    /// Gets or sets the thickness to draw the icon with.
    /// </summary>
    public double Thickness { get; set; } = 0.3;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not TKind)
            return null;

        var foregroundBrush = parameter as Brush ?? Brushes.Black;
        return CreateImageSource(value, foregroundBrush, Thickness);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) 
        => throw new NotImplementedException();

    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    protected abstract ImageSource? CreateImageSource(object value, Brush foregroundBrush, double penThickness);
}