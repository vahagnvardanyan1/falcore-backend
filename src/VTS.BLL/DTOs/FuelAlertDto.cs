namespace VTS.BLL.DTOs;

public class FuelAlertDto
{
    public long Id { get; set; }
    public long VehicleId { get; set; }
    public string Name { get; set; } = null!;
    public double ThresholdValue { get; set; }
    public FuelAlertTypeDto AlertType { get; set; }
    public bool Triggered { get; set; }
}

public enum FuelAlertTypeDto
{
    Low = 0,
    High = 1
}
