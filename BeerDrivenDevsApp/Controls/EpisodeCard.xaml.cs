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

}