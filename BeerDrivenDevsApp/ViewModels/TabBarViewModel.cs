using CommunityToolkit.Mvvm.ComponentModel;

namespace BeerDrivenDevsApp.ViewModels;

public partial class TabBarViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string PlayingEpisodeTitle { get; set; } = "Pick an episode";

    [ObservableProperty]
    public partial bool CanPlay { get; set; } = false;
    
    [ObservableProperty]
    public partial bool IsPlaying { get; set; } = false;

    
}