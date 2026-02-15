using AutoMapper;
using VTS.BLL.DTOs;
using VTS.DAL.Entities;
using VTS.BLL.Interfaces;
using VTS.DAL.Interfaces;
using VTS.BLL.Exceptions;

namespace VTS.BLL.Services;

public class NotificationService(INotificationRepository repo, IMapper mapper) : INotificationService
{
    public async Task<IEnumerable<NotificationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await repo.GetAllAsync(cancellationToken);
        return entities.Select(mapper.Map<NotificationDto>);
    }

    public async Task<NotificationDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : mapper.Map<NotificationDto>(entity);
    }

    public async Task<NotificationDto> CreateAsync(NotificationDto dto, CancellationToken cancellationToken = default)
    {
        var entity = mapper.Map<Notification>(dto);
        var created = await repo.AddAsync(entity, cancellationToken);
        return mapper.Map<NotificationDto>(created);
    }

    public async Task<IEnumerable<NotificationDto>> GetByTenantAsync(long tenantId, CancellationToken cancellationToken = default)
    {
        var entities = await repo.GetByTenantAsync(tenantId, cancellationToken);
        return entities.Select(mapper.Map<NotificationDto>);
    }

    public async Task<IEnumerable<NotificationDto>> GetByVehicleAsync(long vehicleId, CancellationToken cancellationToken = default)
    {
        var entities = await repo.GetByVehicleAsync(vehicleId, cancellationToken);
        return entities.Select(mapper.Map<NotificationDto>);
    }

    public async Task MarkAsReadAsync(long notificationId, CancellationToken cancellationToken = default)
    {
        var notification = await repo.GetByIdAsync(notificationId, cancellationToken);

        if (notification == null)
        {
            throw new NotFoundException($"Notification with ID {notificationId} not found.");
        }

        notification.IsRead = true;
        await repo.UpdateAsync(notification, cancellationToken);
    }
}
