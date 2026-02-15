using VTS.DAL.Entities.Core;

namespace VTS.DAL.Entities;

public class VehicleInsurance : BaseEntity
{
    public long VehicleId { get; set; }
    public string Provider { get; set; } = null!;
    public DateOnly? ExpiryDate { get; set; }
}
