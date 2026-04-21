using OrderonTailoringCostCalculation.ViewModels;

namespace OrderonTailoringCostCalculation.Views;

public partial class ComplicatedElementsCount : ContentPage
{
	public ComplicatedElementsCount(ComplicatedElementsCountViewModel complicatedElementsCountViewModel)
	{
		InitializeComponent();

		BindingContext = complicatedElementsCountViewModel;
	}
}