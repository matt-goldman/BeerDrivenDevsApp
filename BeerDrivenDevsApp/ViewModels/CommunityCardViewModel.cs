using CommunityToolkit.Mvvm.ComponentModel;

namespace BeerDrivenDevsApp.ViewModels;

public partial class CommunityCardViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string Thumbnail { get; set; } = string.Empty;
    
    [ObservableProperty]
    public partial int EpisodeNumber { get; set; }
    
    [ObservableProperty]
    public partial string Title { get; set; } = string.Empty;
    
    [ObservableProperty]
    public partial DateTime ReleaseDate { get; set; }
    
    [ObservableProperty]
    public partial int CommentCount { get; set; }
}