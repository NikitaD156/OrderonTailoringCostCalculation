namespace OrderonTailoringCostCalculation
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("ReceiptPage", typeof(Views.ReceiptPage));
        }
    }
}
