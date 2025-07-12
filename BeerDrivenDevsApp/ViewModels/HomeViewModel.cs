using BeerDrivenDevsApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BeerDrivenDevsApp.ViewModels;

public partial class HomeViewModel(IEpisodeService episodes) : ObservableObject
{
    [ObservableProperty]
    private bool _isRefreshing = false;

    public ObservableCollection<EpisodeViewModel> LatestEpisodes { get; set; } = [];
    
    public Task Init() =>
        LoadLatestEpisodes();

    [RelayCommand]
    private async Task Refresh()
    {
        if (IsRefreshing)
            return;
        await LoadLatestEpisodes();
        IsRefreshing = false;
    }

    private async Task LoadLatestEpisodes()
    {
        IsRefreshing = true;
        var latestEpisodes = await episodes.GetLatestEpisodes();
        LatestEpisodes.Clear();
        latestEpisodes.ForEach(e => LatestEpisodes.Add(e));
        IsRefreshing = false;
    }

    // Need to set allow concurrent executions to true to allow multiple downloads at the same time
    [RelayCommand(AllowConcurrentExecutions = true)]
    private async Task DownloadEpisode(EpisodeViewModel episode)
    {
        if (episode.IsDownloaded)
            return;

        // IsDownloading cannot be set directly as it is a computed property
        // Instead, we set DownloadProgress to a non-zero value
        episode.DownloadProgress = 0.5;
        await episodes.DownloadEpisode(episode.EpisodeNumber, episode.DownloadProgress);
        episode.IsDownloaded = true;
    }
}