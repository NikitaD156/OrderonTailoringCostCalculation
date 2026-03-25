using OrderonTailoringCostCalculation.ViewModels;

namespace OrderonTailoringCostCalculation.Views;

public partial class ReceiptPage : ContentPage
{
	public ReceiptPage(ReceiptDetailsViewModel receiptDetailsViewModel)
	{
		InitializeComponent();

		BindingContext = receiptDetailsViewModel;
	}

}