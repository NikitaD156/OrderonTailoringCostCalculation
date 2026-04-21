using OrderonTailoringCostCalculation.ViewModels;

namespace OrderonTailoringCostCalculation.Views;

public partial class MinValueGarmentsPage : ContentPage
{
	public MinValueGarmentsPage(MinValutGarmentsViewModel minValutGarmentsViewModel)
	{
		InitializeComponent();

		BindingContext = minValutGarmentsViewModel;

	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MinValutGarmentsViewModel viewModel)
        {
            await viewModel.LoadAsync();
        }
    }
}