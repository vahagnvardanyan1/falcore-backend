using VTS.DAL.Entities.Core;

namespace VTS.DAL.Entities;

public class Notification : BaseEntity
{
    public long? TenantId { get; set; }
    public long VehicleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime TimestampUtc { get; set; }
    public bool IsRead { get; set; } = false;

    public Tenant Tenant { get; set; }
    public Vehicle Vehicle { get; set; }
}
