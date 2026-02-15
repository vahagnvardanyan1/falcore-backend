using VTS.BLL.DTOs;

namespace VTS.BLL.Interfaces;

public interface IGeofenceService
{
    Task<IEnumerable<GeofenceDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<GeofenceDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<GeofenceDto> CreateAsync(GeofenceDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, GeofenceDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<GeofenceDto>> GetByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<GeofenceDto>> FindContainingPointAsync(double latitude, double longitude, CancellationToken cancellationToken = default);
}
