using BeerDrivenDevsApp.ViewModels;

namespace BeerDrivenDevsApp.Pages;

public partial class CommunityPage : ContentPage
{
    public CommunityPage(CommunityViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}