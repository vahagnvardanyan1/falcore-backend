using VTS.DAL.Entities.Core;

namespace VTS.DAL.Entities;

public class GpsPosition : BaseEntity
{
    public long VehicleId { get; set; }

    // SRID 4326
    public Point Location { get; set; } = null!;
    public DateTime TimestampUtc { get; set; }

    public double? SpeedKph { get; set; }
    public bool? EngineOn { get; set; }

    public double? FuelLevelLiters { get; set; }
}
