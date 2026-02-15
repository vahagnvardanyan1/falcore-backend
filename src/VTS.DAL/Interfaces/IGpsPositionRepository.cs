using VTS.DAL.Entities;

namespace VTS.DAL.Interfaces;

public interface IGpsPositionRepository
{
    Task<GpsPosition> Create(GpsPosition position, CancellationToken cancellationToken);

    Task<GpsPosition> GetLastPositionByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default);
}
