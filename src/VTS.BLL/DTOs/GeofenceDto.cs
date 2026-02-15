namespace VTS.BLL.DTOs;

public class GeofenceDto
{
    public long Id { get; set; }
    public long VehicleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public PointDto Center { get; set; } = new();
    public double RadiusMeters { get; set; }
    public bool Triggered { get; set; }
}

public class PointDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
