using BeerDrivenDevsApp.Utils;
using BeerDrivenDevsApp.ViewModels;

namespace BeerDrivenDevsApp.Services;

public interface IEpisodeService
{
    Task<List<EpisodeViewModel>> GetEpisodes(CancellationToken cancellationToken = default);

    Task<List<EpisodeViewModel>> GetLatestEpisodes(CancellationToken cancellationToken = default);
    
    Task<EpisodeViewModel?> GetEpisode(int episodeNumber, CancellationToken cancellationToken = default);

    Task UpdateEpisode(EpisodeViewModel episode, CancellationToken cancellationToken = default);

    Task DownloadEpisode(int episode, IProgress<double> progress, CancellationToken cancellationToken);
}

public class EpisodeService(
    HttpClient httpClient,
    DataService dataService,
    IFileDownloadService downloads) : IEpisodeService
{
    private static readonly string EpisodeDirectory = Path.Combine(FileSystem.AppDataDirectory, "bdd_episodes");

    public async Task DownloadEpisode(int episodeNumber, IProgress<double> progress, CancellationToken cancellationToken)
    {
        var episode = await dataService.GetEpisode(episodeNumber, cancellationToken);
        
        if (episode == null)
            // TODO: Handle this more gracefully in the UI
            throw new ArgumentException($"Episode with number {episodeNumber} not found.");

        if (episode.IsDownloaded)
            return;

        var downloadUrl = episode.AudioUrl;

        if (string.IsNullOrEmpty(downloadUrl))
            // TODO: Handle this more gracefully in the UI
            throw new ArgumentException("Episode audio URL is not set.", downloadUrl);

        var urlParts = downloadUrl.Split('/');
        if (urlParts.Length < 2 || !urlParts[^1].EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
            // TODO: Handle this more gracefully in the UI
            throw new ArgumentException("Episode audio URL is not valid or does not point to an MP3 file.", downloadUrl);

        var fileName = urlParts[^1];

        // url decode the filename to handle any encoded characters
        fileName = Uri.UnescapeDataString(fileName);

        // replace spaces and special characters in the filename
        fileName = fileName.Replace(" ", "_").Replace("%20", "_").Replace("'", "").Replace("\"", "").Replace("?", "").Replace("&", "").Replace("=", "");

        // Ensure the episode directory exists
        if (!Directory.Exists(EpisodeDirectory))
        {
            Directory.CreateDirectory(EpisodeDirectory);
        }

        episode.AudioFilePath = Path.Combine(EpisodeDirectory, fileName);

        await downloads.DownloadFileAsync(downloadUrl, episode.AudioFilePath, progress, cancellationToken);
        episode.IsDownloaded = true;
        await dataService.UpsertEpisode(episode, cancellationToken);
    }

    public async Task<List<EpisodeViewModel>> GetEpisodes(CancellationToken cancellationToken = default)
    {
        var dbEpisodes = await dataService.GetEpisodes(cancellationToken);
        
        // Convert the database episodes to view models
        return dbEpisodes.Select(e => e.ToViewModel()).ToList();
    }

    public async Task<List<EpisodeViewModel>> GetLatestEpisodes(CancellationToken cancellationToken = default)
    {
        var rssFeed = await httpClient.GetStreamAsync("/episodes/index.xml", cancellationToken);
        using var reader = new StreamReader(rssFeed);
        var testData = await reader.ReadToEndAsync(cancellationToken);

        var episodes = BddFeedDeserializer.DeserializeFeed(testData);

        if (episodes.Count != 0)
        {
            await dataService.AddMissingEpisodes(episodes, cancellationToken);
        }

        var latestDbEpisodes = await dataService.GetLatestEpisodes(6, cancellationToken);

        return latestDbEpisodes.Select(e => e.ToViewModel()).ToList();
    }

    public async Task<EpisodeViewModel?> GetEpisode(int episodeNumber, CancellationToken cancellationToken = default)
    {
        var episode = await dataService.GetEpisode(episodeNumber, cancellationToken);

        return episode?.ToViewModel();
    }

    public Task UpdateEpisode(EpisodeViewModel episode, CancellationToken cancellationToken = default)
    { 
        var dbEpisode = episode.ToModel();

        return dataService.UpsertEpisode(dbEpisode, cancellationToken);
    }
}