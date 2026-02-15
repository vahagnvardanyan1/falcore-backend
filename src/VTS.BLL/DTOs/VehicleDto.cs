namespace VTS.BLL.DTOs;

public class VehicleDto
{
    public int Id { get; set; }
    public string PlateNumber { get; set; } = string.Empty;
    public string VIN { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public int TotalMileage { get; set; }
    public int TenantId { get; set; }
}
