using OrderonTailoringCostCalculation.ViewModels;

namespace OrderonTailoringCostCalculation.Views;

public partial class MaterialGroupsPage : ContentPage
{
	public MaterialGroupsPage(MaterialGroupsViewModel materialGroupsViewModel)
	{
		InitializeComponent();

        BindingContext = materialGroupsViewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MaterialGroupsViewModel viewModel)
        {
            await viewModel.LoadAsync();
        }
    }
}