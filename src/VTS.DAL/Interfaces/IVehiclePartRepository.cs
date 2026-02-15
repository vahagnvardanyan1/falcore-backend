using VTS.DAL.Entities;

namespace VTS.DAL.Interfaces;

public interface IVehiclePartRepository
{
    Task<IEnumerable<VehiclePart>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<VehiclePart> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<VehiclePart> AddAsync(VehiclePart part, CancellationToken cancellationToken = default);
    Task UpdateAsync(VehiclePart part, CancellationToken cancellationToken = default);
    Task DeleteAsync(VehiclePart part, CancellationToken cancellationToken = default);
    Task<IEnumerable<VehiclePart>> GetByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default);
}
