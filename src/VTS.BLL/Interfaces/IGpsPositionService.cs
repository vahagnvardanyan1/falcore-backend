using VTS.BLL.DTOs;

namespace VTS.BLL.Interfaces;

public interface IGpsPositionService
{
    Task<double> GetDrivenDistance(
            long vehicleId,
            DateTime? start = null,
            DateTime? end = null,
            CancellationToken cancellationToken = default);

    Task<(DateTime LastStopTime, double DistanceSinceStop)> GetDistanceFromLastStop(
        long vehicleId,
        CancellationToken cancellationToken = default);

    Task<GpsPositionDto> Create(GpsPositionDto gpsPosition, CancellationToken cancellationToken = default);
}
