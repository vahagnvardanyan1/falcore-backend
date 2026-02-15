using VTS.BLL.DTOs;

namespace VTS.BLL.Interfaces;

public interface IVehicleInsuranceService
{
    Task<IEnumerable<VehicleInsuranceDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<VehicleInsuranceDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<VehicleInsuranceDto> CreateAsync(VehicleInsuranceDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, VehicleInsuranceDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<VehicleInsuranceDto>> GetByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default);
}