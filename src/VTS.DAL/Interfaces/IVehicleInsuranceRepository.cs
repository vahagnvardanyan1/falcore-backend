using VTS.DAL.Entities;

namespace VTS.DAL.Interfaces;

public interface IVehicleInsuranceRepository
{
    Task<IEnumerable<VehicleInsurance>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<VehicleInsurance> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<VehicleInsurance> AddAsync(VehicleInsurance insurance, CancellationToken cancellationToken = default);
    Task UpdateAsync(VehicleInsurance insurance, CancellationToken cancellationToken = default);
    Task DeleteAsync(VehicleInsurance insurance, CancellationToken cancellationToken = default);
    Task<IEnumerable<VehicleInsurance>> GetByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default);
}