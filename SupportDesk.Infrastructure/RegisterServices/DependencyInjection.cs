using Microsoft.Extensions.DependencyInjection;
using SupportDesk.Application.Interfaces;
using SupportDesk.Infrastructure.Data;
using SupportDesk.Infrastructure.Repository;

namespace SupportDesk.Infrastructure.RegisterServices;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<JsonFileHandler>();
        services.AddSingleton<ICustomerRepository, JsonCustomerRepository>();
        services.AddSingleton<ITicketRepository, JsonTicketRepository>();

        return services;
    }
}
