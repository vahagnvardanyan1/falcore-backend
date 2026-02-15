using VTS.BLL.DTOs;
using VTS.DAL.Entities;
using VTS.BLL.Interfaces;
using VTS.BLL.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace VTS.API.Jobs;

public class VehicleTechnicalInspectionJob(
    IServiceProvider serviceProvider,
    ILogger<VehicleTechnicalInspectionJob> logger,
    IOptions<JobSchedulingSettings> jobSettings) : BackgroundService
{
    private readonly JobSchedulingSettings _jobSettings = jobSettings.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("VehicleTechnicalInspectionJob started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VTSContext>();
                var notificationBroadcaster = scope.ServiceProvider.GetRequiredService<INotificationBroadcaster>();

                var nowUtc = DateTime.UtcNow;
                var todayUtc = nowUtc.Date;
                var windowStart = todayUtc;
                var windowEndExclusive = todayUtc.AddDays(8);

                var startDate = DateOnly.FromDateTime(windowStart);
                var endDate = DateOnly.FromDateTime(windowEndExclusive);

                var dueItems = await (
                    from insp in db.VehicleTechnicalInspections.AsNoTracking()
                    join v in db.Vehicles.AsNoTracking() on insp.VehicleId equals v.Id
                    where insp.ExpiryDate >= startDate
                       && insp.ExpiryDate < endDate
                    select new { Inspection = insp, Vehicle = v }
                ).ToListAsync(stoppingToken);

                if (dueItems.Count != 0)
                {
                    logger.LogInformation("Found {Count} vehicles with technical inspection expiring within 7 days (between {Start} and {EndExclusive}).", dueItems.Count, windowStart, windowEndExclusive);

                    var notifications = new List<NotificationDto>();

                    foreach (var item in dueItems)
                    {
                        var expiryDate = item.Inspection.ExpiryDate;
                        var today = DateOnly.FromDateTime(todayUtc);

                        var daysLeft = expiryDate
                            .ToDateTime(TimeOnly.MinValue)
                            .Subtract(today.ToDateTime(TimeOnly.MinValue))
                            .Days;

                        string message;
                        if (daysLeft < 0)
                        {
                            message = $"Vehicle {item.Vehicle.PlateNumber} (ID {item.Vehicle.Id}) technical inspection expired on {expiryDate:yyyy-MM-dd} UTC.";
                        }
                        else if (daysLeft == 0)
                        {
                            message = $"Vehicle {item.Vehicle.PlateNumber} (ID {item.Vehicle.Id}) technical inspection expires today ({expiryDate:yyyy-MM-dd} UTC).";
                        }
                        else if (daysLeft == 1)
                        {
                            message = $"Vehicle {item.Vehicle.PlateNumber} (ID {item.Vehicle.Id}) technical inspection expires in 1 day ({expiryDate:yyyy-MM-dd} UTC).";
                        }
                        else
                        {
                            message = $"Vehicle {item.Vehicle.PlateNumber} (ID {item.Vehicle.Id}) technical inspection expires in {daysLeft} days ({expiryDate:yyyy-MM-dd} UTC).";
                        }

                        notifications.Add(new NotificationDto
                        {
                            TenantId = item.Vehicle.TenantId,
                            VehicleId = item.Vehicle.Id,
                            Title = "Technical inspection reminder",
                            Message = message,
                            TimestampUtc = DateTime.UtcNow
                        });
                    }

                    if (notifications.Count > 0)
                    {
                        await notificationBroadcaster.BroadcastAsync(notifications, stoppingToken);
                    }
                }
                else
                {
                    logger.LogDebug("No vehicles with technical inspection expiring within 7 days (starting {Start}).", windowStart);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while running VehicleTechnicalInspectionJob.");
            }

            try
            {
                var delay = TimeSpan.FromHours(_jobSettings.VehicleInsuranceCheckIntervalHours);
                await Task.Delay(delay, stoppingToken);
            }
            catch (TaskCanceledException) { }
        }

        logger.LogInformation("VehicleTechnicalInspectionJob stopped.");
    }
}
