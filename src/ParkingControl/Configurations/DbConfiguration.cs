using EntityFrameworkCore.UnitOfWork.Extensions;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ParkingControl.Configurations;

public static class DbConfiguration
{
    public static void AddDbConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"),
                            b => b.MigrationsAssembly("Infrastructure.Data"));
        }, contextLifetime: ServiceLifetime.Transient);

        services.AddUnitOfWork<DataContext>(ServiceLifetime.Transient);
    }
}