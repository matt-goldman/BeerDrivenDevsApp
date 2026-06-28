using BeerDrivenDevsApp.Models;

namespace BeerDrivenDevsApp.Services;

public interface IAudioStateService
{
    State<AudioState> CurrentEpisode { get; }
    
    void PlayEpisode(int episodeId);

    void Pause();

    void Resume();
}

public class AudioStateService : IAudioStateService
{
    public State<AudioState> CurrentEpisode { get; } = new(new AudioState { EpisodeId = 0, Status =  PlayingStatus.Stopped });
    
    public void PlayEpisode(int episodeId)
    {
        var currentState = CurrentEpisode.CurrentValue;
        
        if (currentState.EpisodeId != episodeId)
        {
            currentState.Status = PlayingStatus.Stopped;
            
            CurrentEpisode.SetValue(currentState);
        }
        
        var newState = new AudioState
        {
            EpisodeId   = episodeId,
            Status      = PlayingStatus.Playing
        };
        
        CurrentEpisode.SetValue(newState);
    }

    public void Pause()
    {
        var state = new AudioState
        {
            Status      = PlayingStatus.Paused,
            EpisodeId   = CurrentEpisode.CurrentValue.EpisodeId
        };
        
        CurrentEpisode.SetValue(state);
    }

    public void Resume()
    {
        var state = new AudioState
        {
            Status      = PlayingStatus.Playing,
            EpisodeId   = CurrentEpisode.CurrentValue.EpisodeId
        };
        
        CurrentEpisode.SetValue(state);
    }
}