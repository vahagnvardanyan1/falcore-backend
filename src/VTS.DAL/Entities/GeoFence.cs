using VTS.DAL.Entities.Core;

namespace VTS.DAL.Entities;

public class GeoFence : BaseEntity
{
    public long VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    public Point Center { get; set; }
    public double RadiusMeters { get; set; }

    public bool Triggered { get; set; }
}
