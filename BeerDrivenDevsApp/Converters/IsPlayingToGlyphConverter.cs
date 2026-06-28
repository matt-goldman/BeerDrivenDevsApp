using System.Globalization;

namespace BeerDrivenDevsApp.Converters;

public class IsPlayingToGlyphConverter : IValueConverter
{
    private readonly string _pauseIcon = (string)Application.Current!.Resources["Pause"];
    private readonly string _playIcon = (string) Application.Current!.Resources["Play"];
    private readonly Color _color = (Color)Application.Current!.Resources["Amber950"];
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isPlaying)
        {
            return new FontImageSource
            {
                Glyph       = isPlaying ? _pauseIcon : _playIcon,
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