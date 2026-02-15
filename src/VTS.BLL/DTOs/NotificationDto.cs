namespace VTS.BLL.DTOs;

public class NotificationDto
{
    public long? TenantId { get; set; }
    public long VehicleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
}