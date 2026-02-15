using VTS.DAL.Entities.Core;

namespace VTS.DAL.Entities;

public class VehicleTechnicalInspection : BaseEntity
{
    public long VehicleId { get; set; }
    public DateOnly ExpiryDate { get; set; }
}
