namespace BeerDrivenDevsApp.Services;

public interface INavigationStateService
{
    void UpdateNavigation();
    event EventHandler Navigated;
}

public class NavigationStateService : INavigationStateService
{
    public void UpdateNavigation() => Navigated?.Invoke(this, EventArgs.Empty);

    public event EventHandler? Navigated;
}