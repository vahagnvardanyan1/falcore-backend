using VTS.DAL.Entities.Core;

namespace VTS.DAL.Entities;

public class UserTenantAccess : BaseEntity
{
    public long UserId { get; set; }
    public long TenantId { get; set; }
    public bool HasAccessToAllVehicles { get; set; } = false;

    public virtual ApplicationUser User { get; set; }
    public virtual Tenant Tenant { get; set; }
}
