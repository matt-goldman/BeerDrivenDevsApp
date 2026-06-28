using BeerDrivenDevsApp.Converters;
using System.Windows.Input;

namespace BeerDrivenDevsApp.Controls;

public partial class EpisodeCard : ContentView
{
    public EpisodeCard()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty PlayCommandProperty =
        BindableProperty.Create(nameof(PlayCommand), typeof(ICommand), typeof(EpisodeCard));

    public ICommand PlayCommand
    {
        get => (ICommand)GetValue(PlayCommandProperty);
        set => SetValue(PlayCommandProperty, value);
    }
    
    public static readonly BindableProperty DownloadCommandProperty =
    BindableProperty.Create(nameof(DownloadCommand), typeof(ICommand), typeof(EpisodeCard));

    public ICommand DownloadCommand
    {
        get => (ICommand)GetValue(DownloadCommandProperty);
        set => SetValue(DownloadCommandProperty, value);
    }

    public static readonly BindableProperty CancelDownloadCommandProperty =
        BindableProperty.Create(nameof(CancelDownloadCommand), typeof(ICommand), typeof(EpisodeCard));

    public ICommand CancelDownloadCommand
    {
        get => (ICommand)GetValue(CancelDownloadCommandProperty);
        set => SetValue(CancelDownloadCommandProperty, value);
    }
}