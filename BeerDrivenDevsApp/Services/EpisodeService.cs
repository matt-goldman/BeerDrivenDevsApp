using BeerDrivenDevsApp.Models;
using BeerDrivenDevsApp.Utils;

namespace BeerDrivenDevsApp.Services;

public interface IEpisodeService
{
    Task<List<Episode>> GetEpisodes();

    Task<List<Episode>> GetLatestEpisodes();
}


public class EpisodeService(HttpClient httpClient, DataService dataService) : IEpisodeService
{
    
    public Task<List<Episode>> GetEpisodes()
    {
        return dataService.GetEpisodes();
    }

    public async Task<List<Episode>> GetLatestEpisodes()
    {
        var rssFeed = await httpClient.GetStreamAsync("/episodes/index.xml");
        using var reader = new StreamReader(rssFeed);
        var testData = await reader.ReadToEndAsync();

        var episodes = BddFeedDeserializer.DeserializeFeed(testData);

        if (episodes != null && episodes.Count != 0)
        {
            await dataService.AddMissingEpisodes(episodes);
        }

        return await dataService.GetLatestEpisodes(6);
    }
}