using VTS.DAL.Entities;
using VTS.DAL.Interfaces;

namespace VTS.DAL.Repositories;

public class GeofenceRepository(VTSContext db) : IGeofenceRepository
{
    public async Task<IEnumerable<GeoFence>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await db.GeoFences.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<GeoFence> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        await db.GeoFences.FindAsync([id], cancellationToken);

    public async Task<GeoFence> AddAsync(GeoFence geofence, CancellationToken cancellationToken = default)
    {
        db.GeoFences.Add(geofence);
        await db.SaveChangesAsync(cancellationToken);
        return geofence;
    }

    public async Task UpdateAsync(GeoFence geofence, CancellationToken cancellationToken = default)
    {
        db.GeoFences.Update(geofence);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(GeoFence geofence, CancellationToken cancellationToken = default)
    {
        db.GeoFences.Remove(geofence);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<GeoFence>> GetByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default) =>
        await db.GeoFences.AsNoTracking().Where(g => g.VehicleId == vehicleId).ToListAsync(cancellationToken);
}
