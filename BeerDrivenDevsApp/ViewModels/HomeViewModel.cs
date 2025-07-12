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

    [RelayCommand]
    private async Task DownloadEpisode(EpisodeViewModel episode)
    {
        if (episode.IsDownloaded)
            return;
        
        await episodes.DownloadEpisode(episode.EpisodeNumber, episode.DownloadProgress);
        episode.IsDownloaded = true;
    }
}