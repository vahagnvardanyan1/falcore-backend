using AutoMapper;
using VTS.BLL.DTOs;
using VTS.DAL.Entities;
using VTS.BLL.Interfaces;
using VTS.DAL.Interfaces;
using VTS.BLL.Exceptions;

namespace VTS.BLL.Services;

public class VehicleInsuranceService(IVehicleInsuranceRepository repo, IVehicleRepository vehicleRepository, IMapper mapper) : IVehicleInsuranceService
{
    public async Task<IEnumerable<VehicleInsuranceDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await repo.GetAllAsync(cancellationToken);
        return entities.Select(mapper.Map<VehicleInsuranceDto>);
    }

    public async Task<VehicleInsuranceDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await repo.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : mapper.Map<VehicleInsuranceDto>(entity);
    }

    public async Task<VehicleInsuranceDto> CreateAsync(VehicleInsuranceDto dto, CancellationToken cancellationToken = default)
    {
        var _ = await vehicleRepository.GetByIdAsync(dto.VehicleId, cancellationToken) ?? throw new NotFoundException($"Vehicle with {dto.VehicleId} was not found");

        var entity = mapper.Map<VehicleInsurance>(dto);
        var created = await repo.AddAsync(entity, cancellationToken);
        return mapper.Map<VehicleInsuranceDto>(created);
    }

    public async Task<bool> UpdateAsync(long id, VehicleInsuranceDto dto, CancellationToken cancellationToken = default)
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

    public async Task<IEnumerable<VehicleInsuranceDto>> GetByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default)
    {
        var entities = await repo.GetByVehicleIdAsync(vehicleId, cancellationToken);
        return entities.Select(mapper.Map<VehicleInsuranceDto>);
    }
}