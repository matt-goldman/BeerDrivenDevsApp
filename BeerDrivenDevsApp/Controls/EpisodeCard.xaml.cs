using BeerDrivenDevsApp.Converters;
using System.Windows.Input;

namespace BeerDrivenDevsApp.Controls;

public partial class EpisodeCard : ContentView
{
    public EpisodeCard()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty DownloadCommandProperty =
    BindableProperty.Create(nameof(DownloadCommand), typeof(ICommand), typeof(EpisodeCard));

    public ICommand DownloadCommand
    {
        get => (ICommand)GetValue(DownloadCommandProperty);
        set => SetValue(DownloadCommandProperty, value);
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (width > 0)
        {
            // Update the converter's ViewWidth property to reflect the new width
            var converter = (ProgressToWidthConverter)Resources["ProgressToWidth"];
            converter.ViewWidth = width;
        }
    }
}