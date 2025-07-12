using System.Globalization;

namespace BeerDrivenDevsApp.Converters;

public class ProgressToWidthConverter : IValueConverter
{
    public double ViewWidth { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not double progress || ViewWidth <= 0)
        {
            return 0.0; // Return 0 if progress is not a double or ViewWidth is not set
        }

        // Ensure progress is between 0 and 1
        progress = Math.Clamp(progress, 0.0, 1.0);

        // Calculate the width based on the progress and the view width
        double width = ViewWidth * progress;

        return width;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
