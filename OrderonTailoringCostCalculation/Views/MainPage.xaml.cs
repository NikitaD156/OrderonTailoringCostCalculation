using OrderonTailoringCostCalculation.ViewModels;

namespace OrderonTailoringCostCalculation.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(ReceiptsViewModel receiptsViewModel)
        {
            InitializeComponent();

            BindingContext = receiptsViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ReceiptsViewModel viewModel)
            {
                await viewModel.LoadAsync();
            }
        }

    }
}
