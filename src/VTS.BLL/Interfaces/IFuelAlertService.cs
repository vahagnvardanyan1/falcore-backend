using VTS.BLL.DTOs;

namespace VTS.BLL.Interfaces;

public interface IFuelAlertService
{
    Task<FuelAlertDto> Create(FuelAlertDto fuelAlert, CancellationToken cancellationToken);
    Task<FuelAlertDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<FuelAlertDto>> GetByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default);
    Task<FuelAlertDto> UpdateAsync(FuelAlertDto fuelAlert, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
}
