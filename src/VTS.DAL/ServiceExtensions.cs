using VTS.DAL.Entities;
using VTS.DAL.Interfaces;
using VTS.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VTS.DAL;

public static class ServiceExtensions
{
    /// <summary>
    /// Add Database context into service collection for DI
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddTrackerContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<VTSContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("Default"),
                o => o.UseNetTopologySuite()
            ));

        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IGpsPositionRepository, GpsPositionRepository>();
        services.AddScoped<IVehicleInsuranceRepository, VehicleInsuranceRepository>();
        services.AddScoped<IVehicleTechnicalInspectionRepository, VehicleTechnicalInspectionRepository>();
        services.AddScoped<IGeofenceRepository, GeofenceRepository>();
        services.AddScoped<IFuelAlertRepository, FuelAlertRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IVehiclePartRepository, VehiclePartRepository>();

        return services;
    }
}
