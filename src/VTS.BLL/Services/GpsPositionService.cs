using AutoMapper;
using VTS.BLL.DTOs;
using VTS.DAL.Entities;
using VTS.BLL.Exceptions;
using VTS.BLL.Interfaces;
using VTS.DAL.Extensions;
using VTS.DAL.Interfaces;
using VTS.BLL.Configuration;
using NetTopologySuite.Geometries;
using Microsoft.EntityFrameworkCore;

namespace VTS.BLL.Services;

public class GpsPositionService(
    IGpsPositionRepository repository,
    IVehicleRepository vehicleRepository,
    VTSContext dbContext,
    DistanceCalculationSettings distanceCalculationSettings,
    IMapper mapper) : IGpsPositionService
{
    public async Task<GpsPositionDto> Create(GpsPositionDto gpsPosition, CancellationToken cancellationToken)
    {
        var vehicle = await vehicleRepository.GetByIdAsync(gpsPosition.VehicleId, cancellationToken) ?? throw new NotFoundException($"Vehicle with {gpsPosition.VehicleId} was not found");
        
        var location = new Point(gpsPosition.Longitude, gpsPosition.Latitude) { SRID = 4326 };
        var position = mapper.Map<GpsPosition>(gpsPosition);
        position.Location = location;

        await repository.Create(position, cancellationToken);
        
        // Update vehicle's total mileage if provided in GPS position
        if (gpsPosition.OdometerKm.HasValue && gpsPosition.OdometerKm.Value > 0)
        {
            vehicle.TotalMileage = gpsPosition.OdometerKm.Value;
            await vehicleRepository.UpdateAsync(vehicle, cancellationToken);
        }

        return mapper.Map<GpsPositionDto>(position);
    }

    public async Task<double> GetDrivenDistance(
        long vehicleId,
        DateTime? start = null,
        DateTime? end = null,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext
           .GpsPositions
           .AsNoTracking()
           .Where(x => x.VehicleId == vehicleId)
           .AsQueryable();

        query = query.WhereIf(start.HasValue, q => q.TimestampUtc >= start.Value);
        query = query.WhereIf(end.HasValue, q => q.TimestampUtc <= end.Value);

        query = query.OrderBy(x => x.TimestampUtc);

        var data = await query.Select(x => new GpsPosition
        {
            Id = x.Id,
            TimestampUtc = x.TimestampUtc,
            Location = x.Location
        }).ToListAsync(cancellationToken);

        var distances = new List<double>();

        for (int i = 1; i < data.Count; i++)
        {
            var prev = data[i - 1];
            var curr = data[i];

            double segmentMeters = await dbContext
                .GpsPositions
                .AsNoTracking()
                .Where(x => x.Id == curr.Id)
                .Select(p => p.Location.Distance(prev.Location))
                .SingleAsync(cancellationToken);

            distances.Add(segmentMeters);
        }

        double totalMeters = 0;
        for (int i = 0; i < distances.Count; i++)
        {
            double segment = distances[i];
            double seconds = (data[i + 1].TimestampUtc - data[i].TimestampUtc).TotalSeconds;

            if (segment < distanceCalculationSettings.MinDistance) continue;
            if (segment > distanceCalculationSettings.MaxDistance) continue;
            if ((segment / seconds) > distanceCalculationSettings.MaxSpeedKmh) continue;

            totalMeters += segment;
        }

        return totalMeters;
    }

    public async Task<(DateTime LastStopTime, double DistanceSinceStop)> GetDistanceFromLastStop(
        long vehicleId,
        CancellationToken cancellationToken = default)
    {
        // Get all positions ordered by timestamp
        var allPositions = await dbContext.GpsPositions
            .AsNoTracking()
            .Where(x => x.VehicleId == vehicleId)
            .OrderByDescending(x => x.TimestampUtc)
            .Select(x => new { x.Id, x.TimestampUtc, x.Location })
            .ToListAsync(cancellationToken);

        if (allPositions.Count == 0)
        {
            return (DateTime.UtcNow, 0);
        }

        if (allPositions.Count < 2)
        {
            return (allPositions[0].TimestampUtc, 0);
        }

        // Find the last stop point by checking segments in reverse
        DateTime lastStopTime = allPositions[0].TimestampUtc;
        int stopPointIndex = 0;

        for (int i = 0; i < allPositions.Count - 1; i++)
        {
            var curr = allPositions[i];
            var next = allPositions[i + 1];

            double segmentMeters = await dbContext
                .GpsPositions
                .AsNoTracking()
                .Where(x => x.Id == curr.Id)
                .Select(p => p.Location.Distance(next.Location))
                .SingleAsync(cancellationToken);

            // Stop found when distance is below minimum threshold
            if (segmentMeters < distanceCalculationSettings.MinDistance)
            {
                lastStopTime = curr.TimestampUtc;
                stopPointIndex = i;
                break;
            }
        }

        // Calculate total distance from last stop point to most recent position
        double distanceSinceStop = 0;

        if (stopPointIndex > 0)
        {
            var stopPointId = allPositions[stopPointIndex].Id;
            var positions = await dbContext.GpsPositions
                .AsNoTracking()
                .Where(x => x.VehicleId == vehicleId && x.Id >= stopPointId)
                .OrderBy(x => x.TimestampUtc)
                .Select(x => new { x.Id, x.TimestampUtc, x.Location })
                .ToListAsync(cancellationToken);

            for (int i = 1; i < positions.Count; i++)
            {
                var prev = positions[i - 1];
                var curr = positions[i];

                double segmentMeters = await dbContext
                    .GpsPositions
                    .AsNoTracking()
                    .Where(x => x.Id == curr.Id)
                    .Select(p => p.Location.Distance(prev.Location))
                    .SingleAsync(cancellationToken);

                double seconds = (curr.TimestampUtc - prev.TimestampUtc).TotalSeconds;

                // Apply validation rules
                if (segmentMeters >= distanceCalculationSettings.MinDistance &&
                    segmentMeters <= distanceCalculationSettings.MaxDistance &&
                    seconds > 0 &&
                    (segmentMeters / seconds) <= distanceCalculationSettings.MaxSpeedKmh)
                {
                    distanceSinceStop += segmentMeters;
                }
            }
        }

        return (lastStopTime, distanceSinceStop);
    }
}
