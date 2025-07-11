using BeerDrivenDevsApp.ViewModels;

namespace BeerDrivenDevsApp.Pages;

public partial class HomePage : ContentPage
{
    private HomeViewModel _viewModel;
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await _viewModel.Init();
    }
}