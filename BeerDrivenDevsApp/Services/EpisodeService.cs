using BeerDrivenDevsApp.Utils;
using BeerDrivenDevsApp.ViewModels;

namespace BeerDrivenDevsApp.Services;

public interface IEpisodeService
{
    Task<List<EpisodeViewModel>> GetEpisodes();

    Task<List<EpisodeViewModel>> GetLatestEpisodes();

    Task UpdateEpisode(EpisodeViewModel episode);

    Task DownloadEpisode(int episode);
}

public class EpisodeService(
    HttpClient httpClient,
    DataService dataService,
    IFileDownloadService downloads) : IEpisodeService
{
    public async Task DownloadEpisode(int episodeNumber)
    {
        var episode = await dataService.GetEpisode(episodeNumber);
        
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

        episode.AudioFilePath = Path.Combine("bdd_episodes", fileName);

        var destinationPath = Path.Combine(FileSystem.AppDataDirectory, episode.AudioFilePath);
        var progress = new Progress<double>(p => episode.DownloadProgress = p);

        await downloads.DownloadFileAsync(downloadUrl, destinationPath, progress, CancellationToken.None);
        episode.IsDownloaded = true;
        episode.DownloadProgress = 1.0; // Set to 100% after download completes
        await dataService.UpsertEpisode(episode);
    }

    public async Task<List<EpisodeViewModel>> GetEpisodes()
    {
        var dbEpisodes = await dataService.GetEpisodes();

        if (dbEpisodes == null || dbEpisodes.Count == 0)
        {
            // If no episodes in the database, fetch from the RSS feed
            return await GetLatestEpisodes();
        }

        // Convert the database episodes to view models
        return dbEpisodes.Select(e => e.ToViewModel()).ToList();
    }

    public async Task<List<EpisodeViewModel>> GetLatestEpisodes()
    {
        var rssFeed = await httpClient.GetStreamAsync("/episodes/index.xml");
        using var reader = new StreamReader(rssFeed);
        var testData = await reader.ReadToEndAsync();

        var episodes = BddFeedDeserializer.DeserializeFeed(testData);

        if (episodes != null && episodes.Count != 0)
        {
            await dataService.AddMissingEpisodes(episodes);
        }

        var latestDbEpisodes = await dataService.GetLatestEpisodes(6);

        return latestDbEpisodes.Select(e => e.ToViewModel()).ToList();
    }

    public Task UpdateEpisode(EpisodeViewModel episode)
    { 
        var dbEpisode = episode.ToModel();

        return dataService.UpsertEpisode(dbEpisode);
    }
}