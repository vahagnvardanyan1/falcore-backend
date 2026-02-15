using AutoMapper;
using VTS.BLL.DTOs;
using VTS.DAL.Entities;
using VTS.BLL.Interfaces;
using VTS.DAL.Interfaces;
using VTS.BLL.Exceptions;

namespace VTS.BLL.Services;

public class VehiclePartService(IVehiclePartRepository repo, IVehicleRepository vehicleRepository, IMapper mapper) : IVehiclePartService
{
    public async Task<IEnumerable<VehiclePartDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await repo.GetAllAsync(cancellationToken);
        return entities.Select(mapper.Map<VehiclePartDto>);
    }

    public async Task<VehiclePartDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : mapper.Map<VehiclePartDto>(entity);
    }

    public async Task<VehiclePartDto> CreateAsync(VehiclePartDto dto, CancellationToken cancellationToken = default)
    {
        var _ = await vehicleRepository.GetByIdAsync(dto.VehicleId, cancellationToken) ?? throw new NotFoundException($"Vehicle with ID {dto.VehicleId} was not found");

        var entity = mapper.Map<VehiclePart>(dto);
        var created = await repo.AddAsync(entity, cancellationToken);
        return mapper.Map<VehiclePartDto>(created);
    }

    public async Task<bool> UpdateAsync(long id, VehiclePartDto dto, CancellationToken cancellationToken = default)
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

    public async Task<IEnumerable<VehiclePartDto>> GetByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default)
    {
        var entities = await repo.GetByVehicleIdAsync(vehicleId, cancellationToken);
        return entities.Select(mapper.Map<VehiclePartDto>);
    }
}
