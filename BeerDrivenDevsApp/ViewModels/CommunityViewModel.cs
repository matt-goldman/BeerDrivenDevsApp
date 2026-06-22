using System.Collections.ObjectModel;
using BeerDrivenDevsApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Plugin.Maui.SmartNavigation.Behaviours;

namespace BeerDrivenDevsApp.ViewModels;

public partial class CommunityViewModel(ICommunityService service) : ObservableObject, IViewModelLifecycle
{
    [ObservableProperty]
    public partial bool IsBusy { get; set; } = false;

    [ObservableProperty]
    public partial string Title { get; set; } = "Community";

    public ObservableCollection<DiscussionCardViewModel> Discussions { get; } = [];

    public async Task OnInitAsync(bool isFirstNavigation)
    {
        if (isFirstNavigation)
        {
            await PopulateSummaries();
        }
    }

    private async Task PopulateSummaries()
    {
        Title = "Getting conversations...";
        IsBusy = true;
        var discussions = await service.GetSummaries();
        
        Discussions.Clear();
        
        var thisYear = DateTime.Now.Year;

        foreach (var discussion in discussions)
        {
            Discussions.Add(new DiscussionCardViewModel
            {
                EpisodeNumber           = discussion.EpisodeId,
                Title                   = discussion.EpisodeTitle,
                Thumbnail               = discussion.Thumbnail,
                CommentCount            = discussion.CommentCount == 0 ? "Start the discussion"
                                                                       : discussion.CommentCount > 1 
                                                                           ? $"{discussion.CommentCount} comments" 
                                                                           : "1 comment",
                ReleaseDate             = discussion.ReleaseDate.Year == thisYear 
                                                                        ? discussion.ReleaseDate.ToString("dd MMMM yyyy")
                                                                        : discussion.ReleaseDate.ToString("MMMM yyyy"),
                IsReleaseDateVisible    = discussion.CommentCount > 0
            });
        }
        
        IsBusy = false;
        Title = "Community";
    }
}