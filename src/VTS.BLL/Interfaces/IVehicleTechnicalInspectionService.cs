using VTS.BLL.DTOs;

namespace VTS.BLL.Interfaces;

public interface IVehicleTechnicalInspectionService
{
    Task<IEnumerable<VehicleTechnicalInspectionDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<VehicleTechnicalInspectionDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<VehicleTechnicalInspectionDto> CreateAsync(VehicleTechnicalInspectionDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, VehicleTechnicalInspectionDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<VehicleTechnicalInspectionDto>> GetByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default);
}
