using CommunityToolkit.Mvvm.ComponentModel;

namespace BeerDrivenDevsApp.ViewModels;

public partial class EpisodeViewModel : ObservableObject
{
    [ObservableProperty]
    public partial int EpisodeNumber { get; set; }
    
    [ObservableProperty]
    public partial bool IsNew { get; set; } = false;

    [ObservableProperty]
    public partial string Title { get; set; }

    [ObservableProperty]
    public partial string Description { get; set; }

    [ObservableProperty]
    public partial string Summary { get; set; }

    [ObservableProperty]
    public partial string Duration { get; set; }

    [ObservableProperty]
    public partial DateTime ReleasedOn { get; set; }

    [ObservableProperty]
    public partial string AudioUrl { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsDownloading))]
    public partial bool IsDownloaded { get; set; }
    
    [ObservableProperty]
    public partial string ThumbnailUrl { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsDownloading))]
    public partial double DownloadProgress { get; set; }

    public bool IsDownloading => !IsDownloaded && DownloadProgress >0;

    public CancellationTokenSource? DownloadCts { get; set; }
    
    [ObservableProperty]
    public partial bool IsPlaying { get; set; } = false;

}
