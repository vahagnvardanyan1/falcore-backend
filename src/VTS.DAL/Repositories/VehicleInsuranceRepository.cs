using VTS.DAL.Entities;
using VTS.DAL.Interfaces;

namespace VTS.DAL.Repositories;

public class VehicleInsuranceRepository(VTSContext db) : IVehicleInsuranceRepository
{
    public async Task<IEnumerable<VehicleInsurance>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await db.VehicleInsurances.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<VehicleInsurance> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        await db.VehicleInsurances.FindAsync([id], cancellationToken);

    public async Task<VehicleInsurance> AddAsync(VehicleInsurance insurance, CancellationToken cancellationToken = default)
    {
        db.VehicleInsurances.Add(insurance);
        await db.SaveChangesAsync(cancellationToken);
        return insurance;
    }

    public async Task UpdateAsync(VehicleInsurance insurance, CancellationToken cancellationToken = default)
    {
        db.VehicleInsurances.Update(insurance);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(VehicleInsurance insurance, CancellationToken cancellationToken = default)
    {
        db.VehicleInsurances.Remove(insurance);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<VehicleInsurance>> GetByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default) =>
        await db.VehicleInsurances.AsNoTracking().Where(x => x.VehicleId == vehicleId).ToListAsync(cancellationToken);
}