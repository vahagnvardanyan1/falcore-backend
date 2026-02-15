using VTS.BLL.DTOs;
using VTS.DAL.Entities;
using VTS.BLL.Interfaces;
using VTS.Common.Utilities;
using VTS.BLL.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace VTS.API.Jobs;

public class GeofenceMonitoringJob(
    IServiceProvider serviceProvider,
    ILogger<GeofenceMonitoringJob> logger,
    IOptions<JobSchedulingSettings> jobSettings) : BackgroundService
{
    private readonly JobSchedulingSettings _jobSettings = jobSettings.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("GeofenceMonitoringJob started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VTSContext>();
                var notificationBroadcaster = scope.ServiceProvider.GetRequiredService<INotificationBroadcaster>();

                var geofences = await db.GeoFences
                    .AsNoTracking()
                    .Include(g => g.Vehicle)
                    .ToListAsync(stoppingToken);

                if (geofences.Count == 0)
                {
                    logger.LogDebug("No geofences configured.");
                }
                else
                {
                    logger.LogInformation("Checking {Count} geofences for vehicle violations.", geofences.Count);

                    var notifications = new List<NotificationDto>();
                    var geofencesToUpdate = new List<GeoFence>();

                    foreach (var geofence in geofences)
                    {
                        var latestPosition = await db.GpsPositions
                            .AsNoTracking()
                            .Where(p => p.VehicleId == geofence.VehicleId)
                            .OrderByDescending(p => p.TimestampUtc)
                            .FirstOrDefaultAsync(stoppingToken);

                        if (latestPosition == null)
                        {
                            logger.LogDebug("No GPS position found for vehicle {VehicleId}.", geofence.VehicleId);
                            continue;
                        }

                        bool isInside = GeospatialHelper.IsPointInCircle(latestPosition.Location, geofence.Center, geofence.RadiusMeters);

                        var wasTriggered = geofence.Triggered;

                        if (isInside && !wasTriggered)
                        {
                            // Vehicle entered geofence
                            logger.LogInformation("Vehicle {VehicleId} entered geofence '{GeofenceName}' (ID: {GeofenceId}).",
                                geofence.VehicleId, geofence.Name, geofence.Id);

                            notifications.Add(new NotificationDto
                            {
                                TenantId = geofence.Vehicle.TenantId,
                                VehicleId = geofence.VehicleId,
                                Title = "Geofence Alert - Entry",
                                Message = $"Vehicle {geofence.Vehicle.PlateNumber} (ID {geofence.VehicleId}) has entered geofence '{geofence.Name}'.",
                                TimestampUtc = DateTime.UtcNow
                            });

                            geofence.Triggered = true;
                            geofencesToUpdate.Add(geofence);
                        }
                        // exit from geofence
                        else if (!isInside && wasTriggered)
                        {
                            logger.LogInformation("Vehicle {VehicleId} exited geofence '{GeofenceName}' (ID: {GeofenceId}).",
                                geofence.VehicleId, geofence.Name, geofence.Id);

                            notifications.Add(new NotificationDto
                            {
                                TenantId = geofence.Vehicle.TenantId,
                                VehicleId = geofence.VehicleId,
                                Title = "Geofence Alert - Exit",
                                Message = $"Vehicle {geofence.Vehicle.PlateNumber} (ID {geofence.VehicleId}) has exited geofence '{geofence.Name}'.",
                                TimestampUtc = DateTime.UtcNow
                            });

                            geofence.Triggered = false;
                            geofencesToUpdate.Add(geofence);
                        }
                    }

                    if (geofencesToUpdate.Count > 0)
                    {
                        foreach (var geofence in geofencesToUpdate)
                        {
                            db.GeoFences.Update(geofence);
                        }

                        await db.SaveChangesAsync(stoppingToken);
                    }

                    if (notifications.Count > 0)
                    {
                        await notificationBroadcaster.BroadcastAsync(notifications, stoppingToken);
                    }
                    else
                    {
                        logger.LogDebug("No geofence detected.");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while running GeofenceMonitoringJob.");
            }

            try
            {
                await Task.Delay(TimeSpan.FromMinutes(_jobSettings.GeofenceMonitoringIntervalMinutes), stoppingToken);
            }
            catch (TaskCanceledException) { }
        }

        logger.LogInformation("GeofenceMonitoringJob stopped.");
    }
}
