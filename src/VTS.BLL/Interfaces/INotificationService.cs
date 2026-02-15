using VTS.BLL.DTOs;

namespace VTS.BLL.Interfaces;

public interface INotificationService
{
    Task<IEnumerable<NotificationDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<NotificationDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<NotificationDto> CreateAsync(NotificationDto dto, CancellationToken cancellationToken = default);
    Task<IEnumerable<NotificationDto>> GetByTenantAsync(long tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<NotificationDto>> GetByVehicleAsync(long vehicleId, CancellationToken cancellationToken = default);
    Task MarkAsReadAsync(long notificationId, CancellationToken cancellationToken = default);
}