using VTS.BLL.DTOs;

namespace VTS.BLL.Interfaces;

public interface IVehicleService
{
    Task<IEnumerable<VehicleDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<VehicleDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<VehicleDto> CreateAsync(VehicleDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, VehicleDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<VehicleDto>> GetByTenantAsync(long tenantId, CancellationToken cancellationToken = default);
    Task<double?> GetFuelLevel(long vehicleId, CancellationToken cancellationToken);
    Task<GpsPositionDto> GetLastPositionAsync(long vehicleId, CancellationToken cancellationToken = default);
}
