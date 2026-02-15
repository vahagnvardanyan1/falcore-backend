using AutoMapper;
using VTS.BLL.DTOs;
using VTS.DAL.Entities;
using VTS.BLL.Interfaces;
using VTS.DAL.Interfaces;

namespace VTS.BLL.Services;

public class VehicleService(IVehicleRepository repo, IMapper mapper, IGpsPositionRepository gpsPositionRepository) : IVehicleService
{
    public async Task<IEnumerable<VehicleDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await repo.GetAllAsync(cancellationToken);
        return entities.Select(mapper.Map<VehicleDto>);
    }

    public async Task<VehicleDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : mapper.Map<VehicleDto>(entity);
    }

    public async Task<VehicleDto> CreateAsync(VehicleDto dto, CancellationToken cancellationToken = default)
    {
        var entity = mapper.Map<Vehicle>(dto);
        var created = await repo.AddAsync(entity, cancellationToken);
        return mapper.Map<VehicleDto>(created);
    }

    public async Task<bool> UpdateAsync(long id, VehicleDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await repo.GetByIdAsync(id, cancellationToken);
        if (existing is null) return false;

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

    public async Task<IEnumerable<VehicleDto>> GetByTenantAsync(long tenantId, CancellationToken cancellationToken = default)
    {
        var entities = await repo.GetByTenantAsync(tenantId, cancellationToken);
        return entities.Select(mapper.Map<VehicleDto>);
    }

    public async Task<double?> GetFuelLevel(long vehicleId, CancellationToken cancellationToken = default)
    {
        var fuelLevel = await repo.GetFuelLevel(vehicleId, cancellationToken);
        return fuelLevel;
    }

    public async Task<GpsPositionDto> GetLastPositionAsync(long vehicleId, CancellationToken cancellationToken = default)
    {
        var position = await gpsPositionRepository.GetLastPositionByVehicleIdAsync(vehicleId, cancellationToken);
        return position is null ? null : mapper.Map<GpsPositionDto>(position);
    }
}
