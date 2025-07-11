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
    
    public Task Init() => Refresh();

    [RelayCommand]
    private async Task Refresh()
    {
        IsRefreshing = true;
        await LoadLatestEpisodes();
        IsRefreshing = false;
    }

    private async Task LoadLatestEpisodes()
    {
        var latestEpisodes = await episodes.GetLatestEpisodes();
        LatestEpisodes.Clear();
        latestEpisodes.ForEach(e => LatestEpisodes.Add(e));
    }
}