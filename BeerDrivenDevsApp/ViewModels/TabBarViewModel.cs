using BeerDrivenDevsApp.Controls;
using BeerDrivenDevsApp.Models;
using BeerDrivenDevsApp.Services;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BeerDrivenDevsApp.ViewModels;

public partial class TabBarViewModel: ObservableObject
{
    private readonly IAudioStateService _audioState;
    private readonly IEpisodeService _episodes;

    public TabBarViewModel(IAudioStateService audioState, IEpisodeService episodes)
    {
        _audioState = audioState;
        _episodes = episodes;
        
        _audioState.CurrentEpisode.Subscribe((state) => _ = HandleEpisodeStateChange(state));
    }
    
    [ObservableProperty]
    public partial string PlayingEpisodeTitle { get; set; } = "Pick an episode";
    
    [ObservableProperty]
    public partial MediaElementState CurrentState { get; set; }

    [ObservableProperty]
    public partial bool CanPlay { get; set; } = false;
    
    [ObservableProperty]
    public partial bool IsPlaying { get; set; } = false;

    [ObservableProperty]
    public partial TimeSpan Duration { get; set; }

    [ObservableProperty]
    public partial TimeSpan CurrentPosition { get; set; }

    [ObservableProperty]
    public partial string CurrentPositionDisplay { get; set; } = "00:00";

    [ObservableProperty]
    public partial string DurationDisplay { get; set; } = "00:00";

    [ObservableProperty]
    public partial double Progress { get; set; }
    
    [ObservableProperty]
    public partial MediaSource? MediaSource { get; set; } = null;

    [ObservableProperty]
    public partial ImageSource Thumbnail { get; set; } = new FontImageSource
    {
        FontFamily  = "Lucide",
        Glyph       = Icons.Beer,
        Color       = (Color)Application.Current!.Resources["Gray700"]
    };

    public event EventHandler? Play;
    public event EventHandler? Pause;
    public event EventHandler? Forward;
    public event EventHandler? Backward;
    public event EventHandler? TitleChanged;
    

    private EpisodeViewModel? _currentEpisode;

    private async Task HandleEpisodeStateChange(AudioState state)
    {
        if (_currentEpisode is null || _currentEpisode.EpisodeNumber != state.EpisodeId)
        {
            _currentEpisode = await _episodes.GetEpisode(state.EpisodeId);

            if (_currentEpisode is null)
            {
                IsPlaying = false;
                CanPlay = false;
                PlayingEpisodeTitle = "Pick an episode";
                Duration = TimeSpan.Zero;
                CurrentPosition = TimeSpan.Zero;
                TitleChanged?.Invoke(this, EventArgs.Empty);
                return;
            }
            
            Thumbnail = _currentEpisode.ThumbnailUrl;
            CanPlay = true;
            PlayingEpisodeTitle = _currentEpisode.Title;
            SetAudioSource();
        }

        var isPlaying = state.Status == PlayingStatus.Playing;

        if (IsPlaying == isPlaying) return;
        
        if (isPlaying)
        {
            Play?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Pause?.Invoke(this, EventArgs.Empty);
        }
        
        IsPlaying = isPlaying;
    }

    [RelayCommand]
    private void PlayPause()
    {
        if (IsPlaying)
        {
            _audioState.Pause();
            Pause?.Invoke(this, EventArgs.Empty);
            IsPlaying = false;
        }
        else
        {
            _audioState.Resume();
            Play?.Invoke(this, EventArgs.Empty);
            IsPlaying = true;
        }
    }

    [RelayCommand]
    private void SkipForward() => Forward?.Invoke(this, EventArgs.Empty);

    [RelayCommand]
    private void SkipBackward() => Backward?.Invoke(this, EventArgs.Empty);

    partial void OnCurrentPositionChanged(TimeSpan value)
    {
        // Position is driven by the MediaElement via its read-only, one-way
        // Position bindable property, so this fires on every playback tick.
        CurrentPositionDisplay = Format(value);
        Progress = Duration.TotalSeconds > 0
            ? value.TotalSeconds / Duration.TotalSeconds
            : 0;

        // TODO: debounce + persist CurrentPosition to the database here.
    }

    partial void OnDurationChanged(TimeSpan value)
    {
        // Re-format both labels so position picks up the hh:mm:ss vs mm:ss
        // decision, which is driven by the total duration.
        DurationDisplay = Format(value);
        CurrentPositionDisplay = Format(CurrentPosition);
    }

    // mm:ss when the episode is under an hour, otherwise hh:mm:ss. The chosen
    // format is based on the total Duration so position and duration stay aligned.
    private string Format(TimeSpan value) =>
        Duration.TotalHours >= 1
            ? value.ToString(@"hh\:mm\:ss")
            : value.ToString(@"mm\:ss");

    private void SetAudioSource()
    {
        if (_currentEpisode is null)
        {
            return;
        }
        
        if (!_currentEpisode.IsDownloaded)
        {
            MediaSource = MediaSource.FromUri(_currentEpisode.AudioUrl);
            return;
        }

        MediaSource = File.Exists(_currentEpisode.AudioFilePath)
            ? MediaSource.FromFile(_currentEpisode.AudioFilePath) 
            : MediaSource.FromUri(_currentEpisode.AudioUrl);
    }
}