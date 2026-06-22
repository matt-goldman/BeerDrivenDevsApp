using BeerDrivenDevsApp.Models;

namespace BeerDrivenDevsApp.Services;

public interface ICommunityService
{
    Task<List<DiscussionSummary>> GetSummaries(CancellationToken cancellationToken = default);
    
    Task<Discussion> GetDiscussion(int discussionId, CancellationToken cancellationToken = default);
}