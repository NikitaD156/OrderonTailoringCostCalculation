using OrderonTailoringCostCalculation.ViewModels;

namespace OrderonTailoringCostCalculation.Views;

public partial class ComplicatedElementsPage : ContentPage
{
	public ComplicatedElementsPage(ComplicatedElementsViewModel complicatedElementsViewModel)
	{
		InitializeComponent();

        BindingContext = complicatedElementsViewModel;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ComplicatedElementsViewModel viewModel)
        {
            await viewModel.LoadAsync();
        }
    }
}