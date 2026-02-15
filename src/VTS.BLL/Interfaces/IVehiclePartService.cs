using VTS.BLL.DTOs;

namespace VTS.BLL.Interfaces;

public interface IVehiclePartService
{
    Task<IEnumerable<VehiclePartDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<VehiclePartDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<VehiclePartDto> CreateAsync(VehiclePartDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, VehiclePartDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<VehiclePartDto>> GetByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default);
}
