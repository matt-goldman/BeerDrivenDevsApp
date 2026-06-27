using BeerDrivenDevsApp.Services;

namespace BeerDrivenDevsApp.Pages;

public abstract class PageBase(INavigationStateService navState) : ContentPage
{
    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        base.OnNavigatingFrom(args);
        navState.UpdateNavigation();
    }
}