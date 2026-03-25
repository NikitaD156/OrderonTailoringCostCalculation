using Microsoft.Extensions.Logging;
using OrderonTailoringCostCalculation.Services;
using OrderonTailoringCostCalculation.ViewModels;
using OrderonTailoringCostCalculation.Views;
using CommunityToolkit.Maui;

namespace OrderonTailoringCostCalculation
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<INavigationService, MauiNavigationService>();
            builder.Services.AddSingleton<IDialogService, MauiDialogService>();

            //builder.Services.AddTransient<ReceiptService>();
            //builder.Services.AddTransient<MinValueGarmentService>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<ReceiptsViewModel>();
            builder.Services.AddTransient<ReceiptListItemViewModel>();

            builder.Services.AddTransient<ReceiptPage>();
            builder.Services.AddTransient<ReceiptDetailsViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}