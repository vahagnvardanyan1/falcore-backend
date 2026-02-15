using VTS.DAL.Entities;

namespace VTS.DAL.Interfaces;

public interface INotificationRepository
{
    Task<IEnumerable<Notification>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Notification> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Notification> AddAsync(Notification notification, CancellationToken cancellationToken = default);
    Task UpdateAsync(Notification notification, CancellationToken cancellationToken = default);
    Task DeleteAsync(Notification notification, CancellationToken cancellationToken = default);
    Task<IEnumerable<Notification>> GetByTenantAsync(long tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Notification>> GetByVehicleAsync(long vehicleId, CancellationToken cancellationToken = default);
}
