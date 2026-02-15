using VTS.BLL.DTOs;

namespace VTS.BLL.Interfaces;

public interface INotificationBroadcaster
{
    Task BroadcastAsync(IEnumerable<NotificationDto> notifications, CancellationToken cancellationToken = default);
}
