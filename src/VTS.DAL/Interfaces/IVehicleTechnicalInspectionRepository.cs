using VTS.DAL.Entities;

namespace VTS.DAL.Interfaces;

public interface IVehicleTechnicalInspectionRepository
{
    Task<IEnumerable<VehicleTechnicalInspection>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<VehicleTechnicalInspection> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<VehicleTechnicalInspection> AddAsync(VehicleTechnicalInspection inspection, CancellationToken cancellationToken = default);
    Task UpdateAsync(VehicleTechnicalInspection inspection, CancellationToken cancellationToken = default);
    Task DeleteAsync(VehicleTechnicalInspection inspection, CancellationToken cancellationToken = default);
    Task<IEnumerable<VehicleTechnicalInspection>> GetByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default);
}
