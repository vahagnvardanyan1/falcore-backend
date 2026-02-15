using VTS.BLL.DTOs;
using VTS.DAL.Entities;
using VTS.BLL.Interfaces;
using VTS.BLL.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace VTS.API.Jobs;

public class FuelAlertMonitoringJob(
    IServiceProvider serviceProvider,
    ILogger<FuelAlertMonitoringJob> logger,
    IOptions<JobSchedulingSettings> jobSettings) : BackgroundService
{
    private readonly JobSchedulingSettings _jobSettings = jobSettings.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("FuelAlertMonitoringJob started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VTSContext>();
                var fuelAlertService = scope.ServiceProvider.GetRequiredService<IFuelAlertService>();
                var notificationBroadcaster = scope.ServiceProvider.GetRequiredService<INotificationBroadcaster>();

                var vehicles = await db.Vehicles
                    .AsNoTracking()
                    .ToListAsync(stoppingToken);

                if (vehicles.Count == 0)
                {
                    logger.LogDebug("No vehicles configured.");
                }
                else
                {
                    logger.LogInformation("Checking fuel alerts for {Count} vehicles.", vehicles.Count);

                    var allNotifications = new List<Notification>();

                    foreach (var vehicle in vehicles)
                    {
                        var latestPosition = await db.GpsPositions
                            .AsNoTracking()
                            .Where(p => p.VehicleId == vehicle.Id)
                            .OrderByDescending(p => p.TimestampUtc)
                            .FirstOrDefaultAsync(stoppingToken);

                        if (latestPosition == null || !latestPosition.FuelLevelLiters.HasValue)
                        {
                            logger.LogDebug("No GPS position with fuel level found for vehicle {VehicleId}.", vehicle.Id);
                            continue;
                        }

                        logger.LogInformation("Checking fuel alerts for vehicle {VehicleId} with fuel level {FuelLevel}L.",
                            vehicle.Id, latestPosition.FuelLevelLiters.Value);

                        var fuelAlerts = await db.FuelAlerts
                            .AsNoTracking()
                            .Where(fa => fa.VehicleId == vehicle.Id)
                            .ToListAsync(stoppingToken);

                        if (fuelAlerts.Count == 0)
                        {
                            logger.LogDebug("No fuel alerts configured for vehicle {VehicleId}.", vehicle.Id);
                            continue;
                        }

                        foreach (var alert in fuelAlerts)
                        {
                            bool shouldTrigger = alert.AlertType == FuelAlertType.Low
                                ? latestPosition.FuelLevelLiters.Value < alert.ThresholdValue
                                : latestPosition.FuelLevelLiters.Value > alert.ThresholdValue;

                            // Create notification if alert should trigger and hasn't been triggered yet
                            if (shouldTrigger && !alert.Triggered)
                            {
                                var notification = new Notification
                                {
                                    TenantId = vehicle.TenantId,
                                    VehicleId = vehicle.Id,
                                    Title = $"Fuel Alert: {alert.Name}",
                                    Message = alert.AlertType == FuelAlertType.Low
                                        ? $"Vehicle {vehicle.PlateNumber} fuel level ({latestPosition.FuelLevelLiters.Value}L) is below the threshold ({alert.ThresholdValue}L)"
                                        : $"Vehicle {vehicle.PlateNumber} fuel level ({latestPosition.FuelLevelLiters.Value}L) is above the threshold ({alert.ThresholdValue}L)",
                                    TimestampUtc = DateTime.UtcNow,
                                    IsRead = false
                                };

                                allNotifications.Add(notification);

                                logger.LogInformation("Fuel alert triggered for vehicle {VehicleId}: {AlertName}. Current level: {FuelLevel}L, Threshold: {Threshold}L",
                                    vehicle.Id, alert.Name, latestPosition.FuelLevelLiters.Value, alert.ThresholdValue);

                                alert.Triggered = true;
                                db.FuelAlerts.Update(alert);
                            }
                            else if (!shouldTrigger && alert.Triggered)
                            {
                                logger.LogInformation("Fuel alert cleared for vehicle {VehicleId}: {AlertName}. Current level: {FuelLevel}L, Threshold: {Threshold}L",
                                    vehicle.Id, alert.Name, latestPosition.FuelLevelLiters.Value, alert.ThresholdValue);

                                alert.Triggered = false;
                                db.FuelAlerts.Update(alert);
                            }
                        }
                    }

                    if (allNotifications.Count > 0)
                    {
                        db.Notifications.AddRange(allNotifications);
                        await db.SaveChangesAsync(stoppingToken);

                        var notificationDtos = allNotifications.Select(n => new NotificationDto
                        {
                            TenantId = n.TenantId,
                            VehicleId = n.VehicleId,
                            Title = n.Title,
                            Message = n.Message,
                            TimestampUtc = n.TimestampUtc,
                            IsRead = n.IsRead
                        }).ToList();

                        await notificationBroadcaster.BroadcastAsync(notificationDtos, stoppingToken);
                    }
                    else
                    {
                        logger.LogDebug("No fuel alerts triggered.");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while running FuelAlertMonitoringJob.");
            }

            try
            {
                await Task.Delay(TimeSpan.FromMinutes(_jobSettings.FuelAlertMonitoringIntervalMinutes), stoppingToken);
            }
            catch (TaskCanceledException) { }
        }

        logger.LogInformation("FuelAlertMonitoringJob stopped.");
    }
}
