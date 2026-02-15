namespace VTS.BLL.DTOs;

public class GpsPositionDto
{
    public long Id { get; set; }
    public long VehicleId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime TimestampUtc { get; set; }

    public int? OdometerKm { get; set; }
    public double? SpeedKph { get; set; }
    public bool? EngineOn { get; set; }
    public double? FuelLevelLiters { get; set; }
}
