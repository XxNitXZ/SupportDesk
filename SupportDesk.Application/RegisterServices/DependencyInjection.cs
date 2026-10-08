using Microsoft.Extensions.DependencyInjection;
using SupportDesk.Application.Interfaces;
using SupportDesk.Application.Services;

namespace SupportDesk.Application.RegisterServices;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<ICustomerService, CustomerService>();
        services.AddTransient<ITicketService, TicketService>();

        return services;
    }
}
