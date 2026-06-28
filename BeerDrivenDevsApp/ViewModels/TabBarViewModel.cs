using BeerDrivenDevsApp.Services;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BeerDrivenDevsApp.ViewModels;

public partial class TabBarViewModel(
    IAudioStateService audioState,
    IEpisodeService episodes): ObservableObject
{
    [ObservableProperty]
    public partial string PlayingEpisodeTitle { get; set; } = "Pick an episode";

    [ObservableProperty]
    public partial bool CanPlay { get; set; } = false;
    
    [ObservableProperty]
    public partial bool IsPlaying { get; set; } = false;

    [ObservableProperty]
    private partial double Duration { get; set; }
    
    [ObservableProperty]
    private partial double CurrentPosition { get; set; }
    
    [ObservableProperty]
    private partial double Progress { get; set; }
    
    [ObservableProperty]
    public partial MediaSource? MediaSource { get; set; } = null;
}