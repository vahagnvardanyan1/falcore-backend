namespace VTS.BLL.DTOs;

public class VehicleTechnicalInspectionDto
{
    public long Id { get; set; }
    public long VehicleId { get; set; }
    public DateOnly ExpiryDate { get; set; }
}
