namespace OrderonTailoringCostCalculation
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("ReceiptPage", typeof(Views.ReceiptPage));
            Routing.RegisterRoute("MinValueGarmentsPage", typeof(Views.MinValueGarmentsPage));
            Routing.RegisterRoute("MaterialGroupsPage", typeof(Views.MaterialGroupsPage));
            Routing.RegisterRoute("ComplicatedElementsPage", typeof(Views.ComplicatedElementsPage));
            Routing.RegisterRoute("ComplicatedElementsCount", typeof(Views.ComplicatedElementsCount));
            Routing.RegisterRoute("DiscountsPage", typeof(Views.DiscountsPage));
        }
    }
}
