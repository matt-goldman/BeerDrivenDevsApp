using BeerDrivenDevsApp.ViewModels;

namespace BeerDrivenDevsApp.Pages;

public partial class HomePage : ContentPage
{
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}