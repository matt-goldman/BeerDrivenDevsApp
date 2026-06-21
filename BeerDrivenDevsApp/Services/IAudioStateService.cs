using BeerDrivenDevsApp.Models;

namespace BeerDrivenDevsApp.Services;

public interface IAudioStateService
{
    State<Episode> CurrentEpisode { get; }
}