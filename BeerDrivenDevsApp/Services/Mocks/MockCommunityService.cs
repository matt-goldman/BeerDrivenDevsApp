using BeerDrivenDevsApp.Models;

namespace BeerDrivenDevsApp.Services.Mocks;

public class MockCommunityService(IEpisodeService episodeService) : ICommunityService
{
    public async Task<List<DiscussionSummary>> GetSummaries(CancellationToken cancellationToken = default)
    {
        var episodes = await episodeService.GetEpisodes(cancellationToken);
        
        var summaries = new List<DiscussionSummary>();

        foreach (var episode in episodes.OrderByDescending(e => e.ReleasedOn).Take(10))
        {
            summaries.Add(new DiscussionSummary
            {
                EpisodeId       = episode.EpisodeNumber,
                EpisodeTitle    = episode.Title,
                ReleaseDate     = DateOnly.FromDateTime(episode.ReleasedOn),
                CommentCount    = Random.Shared.Next(0, 10),
                Thumbnail       = episode.ThumbnailUrl
            });
        }
        
        return summaries;
    }

    public Task<Discussion> GetDiscussion(int discussionId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}