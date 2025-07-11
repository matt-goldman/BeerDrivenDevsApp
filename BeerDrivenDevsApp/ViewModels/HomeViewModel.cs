using System.Collections.ObjectModel;
using BeerDrivenDevsApp.Models;
using BeerDrivenDevsApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BeerDrivenDevsApp.ViewModels;

public partial class HomeViewModel(IEpisodeService episodes, IFileDownloadService downloads) : ObservableObject
{
    [ObservableProperty]
    private bool _isRefreshing = false;

    public ObservableCollection<Episode> LatestEpisodes { get; set; } = [];
    
    public async Task Init()
    {
        IsRefreshing = true;
        await LoadLatestEpisodes();
        IsRefreshing = false;
    }
    
    [RelayCommand]
    private Task Refresh() => LoadLatestEpisodes();

    private async Task LoadLatestEpisodes()
    {
        var latestEpisodes = await episodes.GetEpisodes();
        LatestEpisodes.Clear();
        foreach (var episode in latestEpisodes
                     .Take(6))
        {
            LatestEpisodes.Add(episode);
        }
    }
}