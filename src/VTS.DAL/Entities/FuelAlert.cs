using VTS.DAL.Entities.Core;

namespace VTS.DAL.Entities;

public class FuelAlert : BaseEntity
{
    public long VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    public string Name { get; set; } = null!;
    public double ThresholdValue { get; set; }

    public FuelAlertType AlertType { get; set; }

    public bool Triggered { get; set; }
}

public enum FuelAlertType
{
    Low = 0,
    High = 1
}
