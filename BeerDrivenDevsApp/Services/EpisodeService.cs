using BeerDrivenDevsApp.Models;
using BeerDrivenDevsApp.Utils;

namespace BeerDrivenDevsApp.Services;

public interface IEpisodeService
{
    Task<List<Episode>> GetEpisodes();
}

// public class EpisodeService
// {
//     
// }

public class /*Mock*/EpisodeService : IEpisodeService
{
    /*
     * Well, looks like Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../Resources") will do the trick. Who woulda thunk? 😉
     */
    private readonly string _testDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../Resources", "test-data.xml");
    
    public async Task<List<Episode>> GetEpisodes()
    {
        await using var stream = await FileSystem.OpenAppPackageFileAsync("test-data.xml");
        using var reader = new StreamReader(stream);
        var testData = await reader.ReadToEndAsync();

        return BddFeedDeserializer.DeserializeFeed(testData);
    }
}