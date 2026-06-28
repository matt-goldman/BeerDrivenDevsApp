namespace BeerDrivenDevsApp.Models;

public class AudioState
{
    public required int EpisodeId { get; set; }
    
    public PlayingStatus Status { get; set; } = PlayingStatus.Stopped;
}

public enum PlayingStatus
{
    Playing,
    Paused,
    Stopped
}