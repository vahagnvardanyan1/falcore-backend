using VTS.DAL.Entities;

namespace VTS.DAL.Interfaces;

public interface IVehicleRepository
{
    Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Vehicle> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Vehicle> AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
    Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
    Task DeleteAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
    Task<IEnumerable<Vehicle>> GetByTenantAsync(long tenantId, CancellationToken cancellationToken = default);
    Task<double?> GetFuelLevel(long vehicleId, CancellationToken cancellationToken = default);
}
