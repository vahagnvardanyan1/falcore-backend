using VTS.DAL.Entities.Core;

namespace VTS.DAL.Entities;

public class VehiclePart : BaseEntity
{
    public long VehicleId { get; set; }

    public string Name { get; set; } = null!;
    public string PartNumber { get; set; } = null!;

    public int LastServiceOdometerKm { get; set; }
    public int NextServiceOdometerKm { get; set; }
}
