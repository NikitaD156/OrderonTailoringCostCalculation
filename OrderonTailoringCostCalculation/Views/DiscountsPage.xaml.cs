using OrderonTailoringCostCalculation.ViewModels;

namespace OrderonTailoringCostCalculation.Views;

public partial class DiscountsPage : ContentPage
{
	public DiscountsPage(DiscountsViewModel discountsViewModel)
	{
		InitializeComponent();

		BindingContext = discountsViewModel;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is DiscountsViewModel viewModel)
        {
            await viewModel.LoadAsync();
        }
    }
}