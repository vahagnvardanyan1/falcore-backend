using VTS.API.Hubs;
using VTS.BLL.DTOs;
using VTS.BLL.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace VTS.API.Services;

public class NotificationBroadcaster(IHubContext<NotificationHub> hub, INotificationService notificationService, ILogger<NotificationBroadcaster> logger) : INotificationBroadcaster
{
    public async Task BroadcastAsync(IEnumerable<NotificationDto> notifications, CancellationToken cancellationToken = default)
    {
        foreach (var notification in notifications)
        {
            try
            {
                await notificationService.CreateAsync(notification, cancellationToken);

                if (notification.TenantId.HasValue)
                {
                    var groupName = $"tenant-{notification.TenantId.Value}";
                    await hub.Clients.Group(groupName).SendAsync("ReceiveNotification", notification, cancellationToken);
                }
                else
                {
                    await hub.Clients.All.SendAsync("ReceiveNotification", notification, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send notification for vehicle {VehicleId}", notification.VehicleId);
            }
        }
    }
}
