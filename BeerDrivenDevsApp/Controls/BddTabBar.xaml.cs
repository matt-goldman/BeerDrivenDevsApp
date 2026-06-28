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
        tabBarViewModel.TitleChanged += HandleTitleChanged;
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

    private void HandleTitleChanged(object? sender, EventArgs e)
    {
        _cts?.Cancel();
        
        _cts = new CancellationTokenSource();
        
        _ = ScrollTitle(_cts.Token);
    }

    private CancellationTokenSource? _cts;

    // Forward (reveal) scroll speed in device-independent pixels per second.
    private const double MarqueeSpeed = 20;
    // Pause at each end of the scroll, in milliseconds.
    private const int MarqueePause = 1500;

    private async Task ScrollTitle(CancellationToken token)
    {
        TitleLabel.TranslationX = 0;

        try
        {
            // Let the new title lay out. The AbsoluteLayout measures the label
            // with AutoSize bounds, i.e. unconstrained, so TitleLabel.Width is
            // the full natural text width once arrangement settles.
            await Task.Delay(50, token);

            var distance = TitleLabel.Width - MarqueeViewport.Width;

            // Nothing to scroll if the title fits within the viewport.
            if (double.IsNaN(distance) || distance <= 0)
            {
                return;
            }

            var forwardDuration = (uint)(distance / MarqueeSpeed * 1000);

            while (!token.IsCancellationRequested)
            {
                await Task.Delay(MarqueePause, token);
                await TitleLabel.TranslateToAsync(-distance, 0, forwardDuration, Easing.Linear);
                await Task.Delay(MarqueePause, token);
                await TitleLabel.TranslateToAsync(0, 0, 500, Easing.Linear);
            }
        }
        catch (TaskCanceledException)
        {
            // A new title started scrolling; reset position for the next run.
            TitleLabel.TranslationX = 0;
        }
    }
}