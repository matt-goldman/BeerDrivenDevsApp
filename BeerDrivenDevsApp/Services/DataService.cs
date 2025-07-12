using BeerDrivenDevsApp.Models;
using LiteDB.Async;

namespace BeerDrivenDevsApp.Services;

public class DataService
{
    private readonly LiteDatabaseAsync _db;

    public DataService()
    {
        const string dbFileName = "BddApp.db";
        
        var dbPath =
            Path.Combine(FileSystem.AppDataDirectory, dbFileName);
        
        _db = new LiteDatabaseAsync(dbPath);
    }

    public async Task<List<Episode>> GetEpisodes()
    {
        var collection = _db.GetCollection<Episode>();
        var results = await collection.FindAllAsync();

        return results.ToList();
    }

    public async Task<Episode?> GetEpisode(int episodeNumber)
    {
        var collection = _db.GetCollection<Episode>();
        var result = await collection.FindOneAsync(x => x.EpisodeNumber == episodeNumber);
        return result;
    }

    public async Task UpsertEpisode(Episode episode)
    {
        var collection = _db.GetCollection<Episode>();

        bool episodeExists = await collection.ExistsAsync(x => x.EpisodeNumber == episode.EpisodeNumber);

        if (episodeExists)
        {
            await collection.UpdateAsync(episode);
        }
        else
        {
            await collection.InsertAsync(episode);
        }

    }

    public async Task AddMissingEpisodes(IEnumerable<Episode> episodes)
    {
        var collection = _db.GetCollection<Episode>();

        var dbEpisodeNumbers = await collection.Query().Select(x => x.EpisodeNumber).ToListAsync();

        var newEpisodes = episodes.Where(e => !dbEpisodeNumbers.Contains(e.EpisodeNumber)).ToList();

        if (newEpisodes.Count != 0)
        {
            await collection.InsertAsync(newEpisodes);
        }
    }

    public Task<List<Episode>> GetLatestEpisodes(int count)
    {
        var collection = _db.GetCollection<Episode>();
        return collection.Query().OrderByDescending(x => x.EpisodeNumber).Limit(count).ToListAsync();
    }
}