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
}