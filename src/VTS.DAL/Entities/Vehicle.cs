using VTS.DAL.Entities.Core;

namespace VTS.DAL.Entities;

public class Vehicle : BaseEntity
{
    public string PlateNumber { get; set; }
    public string VIN { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public int TotalMileage { get; set; }

    public long TenantId { get; set; }
    public Tenant Tenant { get; set; }

    public ICollection<GpsPosition> Positions { get; set; } = [];
    public ICollection<VehiclePart> Parts { get; set; } = [];
    public ICollection<VehicleInsurance> Insurances { get; set; } = [];
    public ICollection<GeoFence> GeoFences { get; set; } = [];
    public ICollection<FuelAlert> FuelAlerts { get; set; } = [];
}
