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
        return await collection.FindAllAsync();
    }
}