using BeerDrivenDevsApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Plugin.Maui.SmartNavigation.Behaviours;

namespace BeerDrivenDevsApp.ViewModels;

public partial class HomeViewModel(IEpisodeService episodes) : ObservableObject, IViewModelLifecycle
{
    [ObservableProperty]
    public partial bool IsRefreshing { get; set; } = false;
    
    [ObservableProperty]
    public partial bool IsBusy { get; set; } = false;

    [ObservableProperty]
    public partial string Title { get; set; } = "Latest Episodes";
    
    public ObservableCollection<EpisodeViewModel> LatestEpisodes { get; set; } = [];

    // Not bound to the UI. While the initial load is running we set IsRefreshing = true to show the
    // RefreshView spinner, which also fires RefreshCommand. This flag lets that auto-triggered Refresh
    // bail out so LoadNewEpisodes doesn't run twice concurrently. The initial-load path owns the
    // IsRefreshing lifecycle and clears it when done.
    private bool _isInitialLoad;
    
    public async Task OnInitAsync(bool isFirstNavigation)
    {
        if (isFirstNavigation)
        {
            await GetEpisodes();
        }
    }

    [RelayCommand]
    private async Task Refresh()
    {
        // Suppress the refresh that the programmatic IsRefreshing = true triggers during initial load;
        // GetEpisodes is already loading and will clear IsRefreshing itself.
        if (_isInitialLoad)
            return;

        await LoadNewEpisodes();
    }

    private async Task GetEpisodes()
    {
        _isInitialLoad = true;
        IsRefreshing = true;

        var existingEpisodes = await episodes.GetEpisodes();
        existingEpisodes.ForEach(e => LatestEpisodes.Add(e));

        await LoadNewEpisodes();

        _isInitialLoad = false;
    }

    private async Task LoadNewEpisodes()
    {
        IsBusy = true;
        Title = "Checking for new episodes";
        
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
        
        Title = "Latest Episodes";
        IsRefreshing = false;
        IsBusy = false;
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