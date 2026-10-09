using Microsoft.Extensions.DependencyInjection;
using SupportDesk.Presentation.Interfaces;
using SupportDesk.Presentation.Services;
using SupportDesk.Presentation.ViewModels;
using SupportDesk.Presentation.Views;

namespace SupportDesk.Presentation.RegisterServices;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services)
    {
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IDialogService, DialogService>();

        // ViewModels
        services.AddTransient<MainViewModel>();
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<CustomersViewModel>();
        services.AddTransient<TicketsViewModel>();
        services.AddTransient<TicketDetailViewModel>();
        services.AddTransient<NewTicketViewModel>();
        services.AddTransient<NewCustomerViewModel>();
        services.AddTransient<CustomerDetailViewModel>();

        // Views
        services.AddTransient<DashboardPage>();
        services.AddTransient<CustomersPage>();
        services.AddTransient<TicketsPage>();
        services.AddTransient<TicketDetailPage>();
        services.AddTransient<NewTicketPage>();
        services.AddTransient<NewCustomerPage>();
        services.AddTransient<CustomerDetailPage>();

        // MainWindow
        services.AddTransient<MainWindow>();

        return services;
    }
}