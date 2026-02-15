namespace VTS.BLL.DTOs;

public class VehicleInsuranceDto
{
    public long Id { get; set; }
    public long VehicleId { get; set; }
    public string Provider { get; set; } = string.Empty;
    public DateOnly ExpiryDate { get; set; }
}