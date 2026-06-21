using CommunityToolkit.Maui.Core;
using FlagstoneUI.Core.Controls;

namespace BeerDrivenDevsApp.Controls;

public partial class BddTabBar : FsTabBarBase
{
    public BddTabBar()
    {
        InitializeComponent();
    }

    protected override Layout TabContainer => TabBar;

    private void Expander_OnExpandedChanged(object? sender, ExpandedChangedEventArgs e)
    {
        _ = ChevronLabel.RotateToAsync(e.IsExpanded ? 180 : 0);
    }
}