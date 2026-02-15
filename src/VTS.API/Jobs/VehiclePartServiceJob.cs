using VTS.BLL.DTOs;
using VTS.DAL.Entities;
using VTS.BLL.Interfaces;
using VTS.BLL.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace VTS.API.Jobs;

public class VehiclePartServiceJob(
    IServiceProvider serviceProvider,
    ILogger<VehiclePartServiceJob> logger,
    IOptions<JobSchedulingSettings> jobSettings) : BackgroundService
{
    private readonly JobSchedulingSettings _jobSettings = jobSettings.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("VehiclePartServiceJob started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VTSContext>();
                var notificationBroadcaster = scope.ServiceProvider.GetRequiredService<INotificationBroadcaster>();

                // Get all vehicle parts that are due for service based on vehicle's total mileage
                var partsToCheck = await (
                    from part in db.VehicleParts.AsNoTracking()
                    join v in db.Vehicles.AsNoTracking() on part.VehicleId equals v.Id
                    where v.TotalMileage >= part.NextServiceOdometerKm
                    select new { Part = part, Vehicle = v }
                ).ToListAsync(stoppingToken);

                var notifications = new List<NotificationDto>();

                foreach (var item in partsToCheck)
                {
                    var part = item.Part;
                    var vehicle = item.Vehicle;

                    var kmOverdue = vehicle.TotalMileage - part.NextServiceOdometerKm;
                    var message = $"Vehicle {vehicle.PlateNumber} (ID {vehicle.Id}) part '{part.Name}' ({part.PartNumber}) is due for service - total mileage at {vehicle.TotalMileage}km (service due at {part.NextServiceOdometerKm}km, {kmOverdue}km overdue).";

                    notifications.Add(new NotificationDto
                    {
                        TenantId = vehicle.TenantId,
                        VehicleId = vehicle.Id,
                        Title = "Part service reminder",
                        Message = message,
                        TimestampUtc = DateTime.UtcNow
                    });
                }

                if (notifications.Count > 0)
                {
                    logger.LogInformation("Found {Count} vehicle parts that require service.", notifications.Count);
                    await notificationBroadcaster.BroadcastAsync(notifications, stoppingToken);
                }
                else
                {
                    logger.LogDebug("No vehicle parts require service at this time.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while running VehiclePartServiceJob.");
            }

            try
            {
                var delay = TimeSpan.FromHours(_jobSettings.VehicleInsuranceCheckIntervalHours);
                await Task.Delay(delay, stoppingToken);
            }
            catch (TaskCanceledException) { }
        }

        logger.LogInformation("VehiclePartServiceJob stopped.");
    }
}
