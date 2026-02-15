using VTS.BLL.Services;
using VTS.BLL.Interfaces;
using VTS.BLL.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VTS.BLL;

public static class ServiceExtensions
{
    public static IServiceCollection AddTrackerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<IGpsPositionService, GpsPositionService>();
        services.AddScoped<IVehicleInsuranceService, VehicleInsuranceService>();
        services.AddScoped<IVehicleTechnicalInspectionService, VehicleTechnicalInspectionService>();
        services.AddScoped<IGeofenceService, GeofenceService>();
        services.AddScoped<IFuelAlertService, FuelAlertService>();
        services.AddScoped<INotificationService, NotificationService>();

        var distanceCalculationSettings = new DistanceCalculationSettings();
        configuration.GetSection(DistanceCalculationSettings.Section).Bind(distanceCalculationSettings);

        services.AddSingleton(distanceCalculationSettings);

        return services;
    }
}
