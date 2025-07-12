using BeerDrivenDevsApp.Models;

namespace BeerDrivenDevsApp.ViewModels;

public static class Mappers
{
    public static EpisodeViewModel ToViewModel(this Episode episode)
    {
        return new EpisodeViewModel
        {
            EpisodeNumber       = episode.EpisodeNumber,
            Title               = episode.Title,
            Description         = episode.Notes,
            Summary             = episode.Summary,
            Duration            = episode.Duration,
            ReleasedOn          = episode.ReleasedOn,
            AudioUrl            = episode.AudioUrl,
            IsDownloaded        = episode.IsDownloaded,
            ThumbnailUrl        = episode.ThumbnailUrl,
            DownloadProgress    = episode.DownloadProgress
        };
    }

    public static Episode ToModel(this EpisodeViewModel viewModel)
    {
        return new Episode
        {
            EpisodeNumber       = viewModel.EpisodeNumber,
            Title               = viewModel.Title,
            Notes               = viewModel.Description,
            Summary             = viewModel.Summary,
            Duration            = viewModel.Duration,
            ReleasedOn          = viewModel.ReleasedOn,
            AudioUrl            = viewModel.AudioUrl,
            IsDownloaded        = viewModel.IsDownloaded,
            ThumbnailUrl        = viewModel.ThumbnailUrl,
            DownloadProgress    = viewModel.DownloadProgress
        };
    }
}
