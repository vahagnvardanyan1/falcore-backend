using VTS.DAL.Entities.Core;

namespace VTS.DAL.Entities;

public class UserVehicleAccess : BaseEntity
{
    public long UserId { get; set; }
    public long VehicleId { get; set; }

    public virtual ApplicationUser User { get; set; }
    public virtual Vehicle Vehicle { get; set; }
}
