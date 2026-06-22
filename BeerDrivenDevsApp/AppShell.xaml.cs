using FlagstoneUI.Core.Controls;

namespace BeerDrivenDevsApp;

public partial class AppShell : FsShell
{
    public AppShell()
    {
        InitializeComponent();
    }

    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);
        NavBar.HandleNavigated();
    } 
}