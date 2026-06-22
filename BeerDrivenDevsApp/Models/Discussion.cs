namespace BeerDrivenDevsApp.Models;

public class DiscussionSummary
{
    public int EpisodeId { get; set; }
    
    public required string EpisodeTitle { get; set; }

    public int CommentCount { get; set; }

    public DateOnly ReleaseDate { get; set; }
}

public class Discussion
{
    public int EpisodeId { get; set; }

    public DateOnly ReleaseDate { get; set; }
    
    // TODO: Add comments
}