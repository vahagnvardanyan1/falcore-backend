using AutoMapper;
using VTS.BLL.DTOs;
using VTS.DAL.Entities;
using VTS.DAL.Interfaces;
using VTS.BLL.Interfaces;
using VTS.Common.Utilities;
using NetTopologySuite.Geometries;

namespace VTS.BLL.Services;

public class GeofenceService(IGeofenceRepository repo, IMapper mapper) : IGeofenceService
{
    private static readonly GeometryFactory GeometryFactory = new(new PrecisionModel(), 4326);

    public async Task<IEnumerable<GeofenceDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await repo.GetAllAsync(cancellationToken);
        return entities.Select(mapper.Map<GeofenceDto>);
    }

    public async Task<GeofenceDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : mapper.Map<GeofenceDto>(entity);
    }

    public async Task<GeofenceDto> CreateAsync(GeofenceDto dto, CancellationToken cancellationToken = default)
    {
        ValidateGeofenceDto(dto);
        var entity = mapper.Map<GeoFence>(dto);
        var created = await repo.AddAsync(entity, cancellationToken);
        return mapper.Map<GeofenceDto>(created);
    }

    public async Task<bool> UpdateAsync(long id, GeofenceDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await repo.GetByIdAsync(id, cancellationToken);
        if (existing is null) return false;

        ValidateGeofenceDto(dto);
        mapper.Map(dto, existing);
        await repo.UpdateAsync(existing, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var existing = await repo.GetByIdAsync(id, cancellationToken);
        if (existing is null) return false;

        await repo.DeleteAsync(existing, cancellationToken);
        return true;
    }

    public async Task<IEnumerable<GeofenceDto>> GetByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default)
    {
        var entities = await repo.GetByVehicleIdAsync(vehicleId, cancellationToken);
        return entities.Select(mapper.Map<GeofenceDto>);
    }

    public async Task<IEnumerable<GeofenceDto>> FindContainingPointAsync(double latitude, double longitude, CancellationToken cancellationToken = default)
    {
        var point = GeometryFactory.CreatePoint(new Coordinate(longitude, latitude));
        var all = await repo.GetAllAsync(cancellationToken);
        
        var containing = all.Where(g => g.Center != null && GeospatialHelper.IsPointInCircle(point, g.Center, g.RadiusMeters)).ToList();
        return containing.Select(mapper.Map<GeofenceDto>);
    }

    private static void ValidateGeofenceDto(GeofenceDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Geofence name is required.");

        if (dto.VehicleId <= 0)
            throw new ArgumentException("Valid VehicleId is required.");

        if (dto.Center is null || double.IsNaN(dto.Center.Latitude) || double.IsNaN(dto.Center.Longitude))
            throw new ArgumentException("Valid center coordinates are required.");

        if (dto.RadiusMeters <= 0)
            throw new ArgumentException("RadiusMeters must be greater than 0.");
    }
}
