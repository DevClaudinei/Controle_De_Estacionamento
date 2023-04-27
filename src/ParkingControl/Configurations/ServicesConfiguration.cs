using AppServices.Services;
using AppServices.Services.Interfaces;
using DomainServices.Services;
using DomainServices.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ParkingControl.Configurations;

public static class ServicesConfiguration
{
    public static void AddServicesConfiguration(this IServiceCollection services)
    {
        services.AddTransient<IParkingAppService, ParkingAppService>();

        services.AddTransient<IParkingService, ParkingService>();

        services.AddTransient<IPriceAppService, PriceAppService>();

        services.AddTransient<IPriceService, PriceService>();
    }
}