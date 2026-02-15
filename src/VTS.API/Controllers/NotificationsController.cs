using VTS.BLL.DTOs;
using VTS.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using VTS.API.Controllers.Core;

namespace VTS.API.Controllers;

public class NotificationsController(INotificationService notificationService, INotificationBroadcaster notificationBroadcaster) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var notifications = await notificationService.GetAllAsync(cancellationToken);
        return Ok(notifications);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var notification = await notificationService.GetByIdAsync(id, cancellationToken);
        if (notification is null) return NotFound();
        return Ok(notification);
    }

    [HttpGet("tenant/{tenantId:long}")]
    public async Task<IActionResult> GetByTenant(long tenantId, CancellationToken cancellationToken)
    {
        var notifications = await notificationService.GetByTenantAsync(tenantId, cancellationToken);
        return Ok(notifications);
    }

    [HttpGet("vehicle/{vehicleId:long}")]
    public async Task<IActionResult> GetByVehicle(long vehicleId, CancellationToken cancellationToken)
    {
        var notifications = await notificationService.GetByVehicleAsync(vehicleId, cancellationToken);
        return Ok(notifications);
    }

    [HttpPost("sample")]
    public async Task<IActionResult> SendSample([FromBody] NotificationDto dto, CancellationToken cancellationToken)
    {
        if (dto is null) return BadRequest();
        await notificationBroadcaster.BroadcastAsync([dto], cancellationToken);
        return Accepted();
    }

    [HttpPost("sample/{tenantId:long}/{vehicleId:long}")]
    public async Task<IActionResult> SendSampleParams(long tenantId, long vehicleId, CancellationToken cancellationToken)
    {
        var dto = new NotificationDto
        {
            TenantId = tenantId,
            VehicleId = vehicleId,
            Title = "Sample notification",
            Message = $"Sample notification for vehicle {vehicleId}",
            TimestampUtc = DateTime.UtcNow
        };

        await notificationBroadcaster.BroadcastAsync([dto], cancellationToken);
        return Accepted();
    }

    [HttpPut("{id}/mark-as-read")]
    public async Task<ActionResult<IActionResult>> MarkAsRead(long id)
    {
        await notificationService.MarkAsReadAsync(id);
        return Ok();
    }
}