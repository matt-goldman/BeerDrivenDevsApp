using BeerDrivenDevsApp.ViewModels;
using CommunityToolkit.Maui.Core;
using FlagstoneUI.Core.Controls;

namespace BeerDrivenDevsApp.Controls;

public partial class BddTabBar : FsTabBarBase
{
    public BddTabBar()
    {
        InitializeComponent();
        InitializeTabContainer();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        if (BindingContext is not (TabBarViewModel tabBarViewModel))
        {
            return;
        }
        
        tabBarViewModel.Play += HandlePlay;
        tabBarViewModel.Pause += HandlePause;
        tabBarViewModel.Forward += HandleSkipForward;
        tabBarViewModel.Backward += HandleSkipBackward;
    }

    protected override Layout TabContainer => TabBar;

    private void Expander_OnExpandedChanged(object? sender, ExpandedChangedEventArgs e)
    {
        _ = e.IsExpanded ? OpenNav() : CloseNav();
    }

    public void HandleNavigated()
    {
        _ = CloseNav();
    }

    private async Task OpenNav()
    {
        var rotateTask = ChevronLabel.RotateToAsync(180);
        var tabTask = TabBar.TranslateToAsync(0, 0);
        await Task.WhenAll(rotateTask, tabTask);
    }

    private async Task CloseNav()
    {
        var rotateTask = ChevronLabel.RotateToAsync(0);
        var tabTask = TabBar.TranslateToAsync(0, 300);
        await Task.WhenAll(rotateTask, tabTask);
        NavExpander.IsExpanded = false;
    }

    private void HandlePlay(object? sender, EventArgs e)
    {
        MediaElement.Play();
    }

    private void HandlePause(object? sender, EventArgs e)
    {
        MediaElement.Pause();
    }

    private void HandleSkipForward(object? sender, EventArgs e)
    {
        
    }

    private void HandleSkipBackward(object? sender, EventArgs e)
    {
        
    }
}