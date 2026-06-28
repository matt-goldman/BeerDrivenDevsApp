using System.Globalization;
using BeerDrivenDevsApp.Controls;

namespace BeerDrivenDevsApp.Converters;

public class IsPlayingToGlyphConverter : IValueConverter
{
    private readonly Color _color = (Color)Application.Current!.Resources["Amber950"];
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isPlaying)
        {
            return new FontImageSource
            {
                Glyph       = isPlaying ? Icons.Pause : Icons.Play,
                FontFamily  = "Lucide",
                Color       = _color
            };
        }
        
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}