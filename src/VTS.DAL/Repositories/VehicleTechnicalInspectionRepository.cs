using VTS.DAL.Entities;
using VTS.DAL.Interfaces;

namespace VTS.DAL.Repositories;

public class VehicleTechnicalInspectionRepository(VTSContext db) : IVehicleTechnicalInspectionRepository
{
    public async Task<IEnumerable<VehicleTechnicalInspection>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await db.VehicleTechnicalInspections.AsNoTracking().OrderByDescending(v => v.CreatedDateUtc).ToListAsync(cancellationToken);

    public async Task<VehicleTechnicalInspection> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        await db.VehicleTechnicalInspections.FindAsync([id], cancellationToken);

    public async Task<VehicleTechnicalInspection> AddAsync(VehicleTechnicalInspection inspection, CancellationToken cancellationToken = default)
    {
        db.VehicleTechnicalInspections.Add(inspection);
        await db.SaveChangesAsync(cancellationToken);
        return inspection;
    }

    public async Task UpdateAsync(VehicleTechnicalInspection inspection, CancellationToken cancellationToken = default)
    {
        db.VehicleTechnicalInspections.Update(inspection);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(VehicleTechnicalInspection inspection, CancellationToken cancellationToken = default)
    {
        db.VehicleTechnicalInspections.Remove(inspection);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<VehicleTechnicalInspection>> GetByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default) =>
        await db.VehicleTechnicalInspections.AsNoTracking().Where(x => x.VehicleId == vehicleId).OrderByDescending(v => v.CreatedDateUtc).ToListAsync(cancellationToken);
}
