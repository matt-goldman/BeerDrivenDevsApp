using BeerDrivenDevsApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BeerDrivenDevsApp.ViewModels;

public partial class HomeViewModel(IEpisodeService episodes) : ObservableObject
{
    [ObservableProperty]
    public partial bool IsRefreshing { get; set; } = false;
    
    [ObservableProperty]
    public partial bool IsCheckingForUpdates { get; set; } = false;

    [ObservableProperty]
    public partial string HeaderLabel { get; set; } = "Latest Episodes";
    
    public ObservableCollection<EpisodeViewModel> LatestEpisodes { get; set; } = [];

    public Task Init() =>
        GetEpisodes();

    [RelayCommand]
    private async Task Refresh()
    {
        await LoadNewEpisodes();
        IsRefreshing = false;
    }

    private async Task GetEpisodes()
    {
        IsRefreshing = true;
        var existingEpisodes = await episodes.GetEpisodes();
        existingEpisodes.ForEach(e => LatestEpisodes.Add(e));
        IsRefreshing = false;

        await LoadNewEpisodes();
    }

    private async Task LoadNewEpisodes()
    {
        IsCheckingForUpdates = true;
        HeaderLabel = "Checking for new episodes";
        
        var latestEpisodes = await episodes.GetLatestEpisodes();

        foreach (var episode in latestEpisodes.Where(e => !LatestEpisodes.Contains(e)))
        {
            episode.IsNew = true;
        }

        if (latestEpisodes.Any(e => e.IsNew))
        {
            LatestEpisodes.Clear();

            foreach (var episode in latestEpisodes.OrderByDescending(e => e.ReleasedOn))
            {
                LatestEpisodes.Add(episode);
            }
        }
        
        HeaderLabel = "Latest Episodes";
        IsRefreshing = false;
        IsCheckingForUpdates = false;
    }

    // Need to set allow concurrent executions to true to allow multiple downloads at the same time
    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task DownloadEpisode(EpisodeViewModel episode)
    {
        if (episode.IsDownloaded || episode.IsDownloading)
            return;

        episode.DownloadCts = new CancellationTokenSource();

        var progress = new Progress<double>(value => episode.DownloadProgress = value);
        await episodes.DownloadEpisode(episode.EpisodeNumber, progress, episode.DownloadCts.Token);
        episode.IsDownloaded = true;
    }

    [RelayCommand]
    private static void CancelDownload(EpisodeViewModel episode)
    {
        if (episode.DownloadCts is not null)
        {
            episode.DownloadCts.Cancel();
            episode.DownloadCts.Dispose();
            episode.DownloadCts = null;
        }
        episode.IsDownloaded = false;
        episode.DownloadProgress = 0;
    }
}