using VTS.API.Jobs;
using VTS.BLL.Configuration;

namespace VTS.API.Extensions;

public static class JobExtensions
{
    public static IServiceCollection SetupJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JobSchedulingSettings>(configuration.GetSection(JobSchedulingSettings.Section));
        services.AddHostedService<VehicleInsuranceJob>();
        services.AddHostedService<GeofenceMonitoringJob>();
        services.AddHostedService<FuelAlertMonitoringJob>();
        services.AddHostedService<VehicleTechnicalInspectionJob>();
        return services;
    }
}
