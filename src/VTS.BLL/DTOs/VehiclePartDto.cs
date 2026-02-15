namespace VTS.BLL.DTOs;

public class VehiclePartDto
{
    public long Id { get; set; }
    public long VehicleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PartNumber { get; set; } = string.Empty;
    public int ServiceIntervalKm { get; set; }
    public int LastServiceOdometerKm { get; set; }
    public int NextServiceOdometerKm { get; set; }
}
