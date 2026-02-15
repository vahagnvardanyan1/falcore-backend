using VTS.DAL.Entities;
using VTS.DAL.Interfaces;

namespace VTS.DAL.Repositories;

public class NotificationRepository(VTSContext db) : INotificationRepository
{
    public async Task<IEnumerable<Notification>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await db.Notifications.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<Notification> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        await db.Notifications.FindAsync([id], cancellationToken);

    public async Task<Notification> AddAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        db.Notifications.Add(notification);
        await db.SaveChangesAsync(cancellationToken);
        return notification;
    }

    public async Task UpdateAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        db.Notifications.Update(notification);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        db.Notifications.Remove(notification);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Notification>> GetByTenantAsync(long tenantId, CancellationToken cancellationToken = default) =>
        await db.Notifications.AsNoTracking().Where(n => n.TenantId == tenantId).ToListAsync(cancellationToken);

    public async Task<IEnumerable<Notification>> GetByVehicleAsync(long vehicleId, CancellationToken cancellationToken = default) =>
        await db.Notifications.AsNoTracking().Where(n => n.VehicleId == vehicleId).ToListAsync(cancellationToken);
}
