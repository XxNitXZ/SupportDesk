using Microsoft.Extensions.DependencyInjection;
using SupportDesk.Presentation.Interfaces;
using SupportDesk.Presentation.Services;
using SupportDesk.Presentation.ViewModels;
using SupportDesk.Presentation.Views;

namespace SupportDesk.Presentation.RegisterServices;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {


        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IDialogService, DialogService>();

        // ViewModels
        services.AddTransient<MainViewModel>();
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<CustomersViewModel>();
        services.AddTransient<TicketsViewModel>();
        services.AddTransient<TicketDetailViewModel>();

        // Views
        services.AddTransient<DashboardPage>();
        services.AddTransient<CustomersPage>();
        services.AddTransient<TicketsPage>();
        services.AddTransient<TicketDetailPage>();

        // MainWindow
        services.AddTransient<MainWindow>();

        return services;
    }
}
