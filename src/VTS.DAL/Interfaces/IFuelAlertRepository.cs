using VTS.DAL.Entities;

namespace VTS.DAL.Interfaces;

public interface IFuelAlertRepository
{
    Task<FuelAlert> Create(FuelAlert fuelAlert, CancellationToken cancellationToken);
    Task<FuelAlert> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<FuelAlert>> GetByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default);
    Task<FuelAlert> UpdateAsync(FuelAlert fuelAlert, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
}
