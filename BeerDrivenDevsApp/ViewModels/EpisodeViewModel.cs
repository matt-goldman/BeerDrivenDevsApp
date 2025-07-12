using CommunityToolkit.Mvvm.ComponentModel;

namespace BeerDrivenDevsApp.ViewModels;

public partial class EpisodeViewModel : ObservableObject
{
    [ObservableProperty]
    private int _episodeNumber;

    [ObservableProperty]
    private string _title;
    
    [ObservableProperty]
    private string _description;

    [ObservableProperty]
    private string _summary;

    [ObservableProperty]
    private string _duration;

    [ObservableProperty]
    private DateTime _releasedOn;

    [ObservableProperty]
    private string _audioUrl;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsDownloading))]
    private bool _isDownloaded;

    [ObservableProperty]
    private string _thumbnailUrl;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsDownloading))]
    private double _downloadProgress;

    public bool IsDownloading => !IsDownloaded && DownloadProgress >0;
}
