using VTS.DAL.Entities;
using VTS.DAL.Interfaces;

namespace VTS.DAL.Repositories;

public class VehiclePartRepository(VTSContext db) : IVehiclePartRepository
{
    public async Task<IEnumerable<VehiclePart>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await db.VehicleParts.AsNoTracking().OrderByDescending(v => v.CreatedDateUtc).ToListAsync(cancellationToken);

    public async Task<VehiclePart> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        await db.VehicleParts.FindAsync([id], cancellationToken);

    public async Task<VehiclePart> AddAsync(VehiclePart part, CancellationToken cancellationToken = default)
    {
        db.VehicleParts.Add(part);
        await db.SaveChangesAsync(cancellationToken);
        return part;
    }

    public async Task UpdateAsync(VehiclePart part, CancellationToken cancellationToken = default)
    {
        db.VehicleParts.Update(part);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(VehiclePart part, CancellationToken cancellationToken = default)
    {
        db.VehicleParts.Remove(part);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<VehiclePart>> GetByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default) =>
        await db.VehicleParts.AsNoTracking().Where(x => x.VehicleId == vehicleId).OrderByDescending(v => v.CreatedDateUtc).ToListAsync(cancellationToken);
}
