using Microsoft.Extensions.DependencyInjection;
using SupportDesk.Presentation.ViewModels;
using SupportDesk.Presentation.Views;

namespace SupportDesk.Presentation.RegisterServices;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        // ViewModels
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<CustomersViewModel>();
        services.AddTransient<TicketsViewModel>();
        services.AddTransient<TicketDetailViewModel>();
        services.AddTransient<NewTicketViewModel>();

        // Views
        services.AddTransient<DashboardPage>();
        services.AddTransient<CustomersPage>();
        services.AddTransient<TicketsPage>();
        services.AddTransient<TicketDetailPage>();
        services.AddTransient<NewTicketPage>();

        // MainWindow
        services.AddTransient<MainWindow>();

        return services;
    }
}
