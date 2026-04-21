using Microsoft.Extensions.Logging;
using OrderonTailoringCostCalculation.Services;
using OrderonTailoringCostCalculation.ViewModels;
using OrderonTailoringCostCalculation.Views;

namespace OrderonTailoringCostCalculation
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<INavigationService, MauiNavigationService>();
            builder.Services.AddSingleton<IDialogService, MauiDialogService>();

            builder.Services.AddTransient<ReceiptService>();
            //builder.Services.AddTransient<MinValueGarmentService>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<ReceiptsViewModel>();
            builder.Services.AddTransient<ReceiptListItemViewModel>();

            builder.Services.AddTransient<ReceiptPage>();
            builder.Services.AddTransient<ReceiptDetailsViewModel>();

            builder.Services.AddTransient<MinValueGarmentsPage>();
            builder.Services.AddTransient<MinValutGarmentsViewModel>();

            builder.Services.AddTransient<MaterialGroupsPage>();
            builder.Services.AddTransient<MaterialGroupsViewModel>();

            builder.Services.AddTransient<ComplicatedElementsPage>();
            builder.Services.AddTransient<ComplicatedElementsViewModel>();

            builder.Services.AddTransient<ComplicatedElementsCount>();
            builder.Services.AddTransient<ComplicatedElementsCountViewModel>();

            builder.Services.AddTransient<DiscountsPage>();
            builder.Services.AddTransient<DiscountsViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}