using VTS.DAL.Entities;

namespace VTS.DAL.Interfaces;

public interface IGeofenceRepository
{
    Task<IEnumerable<GeoFence>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<GeoFence> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<GeoFence> AddAsync(GeoFence geofence, CancellationToken cancellationToken = default);
    Task UpdateAsync(GeoFence geofence, CancellationToken cancellationToken = default);
    Task DeleteAsync(GeoFence geofence, CancellationToken cancellationToken = default);
    Task<IEnumerable<GeoFence>> GetByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default);
}
