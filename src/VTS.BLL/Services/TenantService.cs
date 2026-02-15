using AutoMapper;
using VTS.BLL.DTOs;
using VTS.DAL.Entities;
using VTS.BLL.Interfaces;
using VTS.DAL.Interfaces;

namespace VTS.BLL.Services;

public class TenantService(ITenantRepository repository, IMapper mapper) : ITenantService
{
    public async Task<IEnumerable<TenantDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await repository.GetAllAsync(cancellationToken);
        return entities.Select(mapper.Map<TenantDto>);
    }

    public async Task<TenantDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : mapper.Map<TenantDto>(entity);
    }

    public async Task<TenantDto> CreateAsync(TenantDto dto, CancellationToken cancellationToken = default)
    {
        var entity = mapper.Map<Tenant>(dto);
        var created = await repository.AddAsync(entity, cancellationToken);
        return mapper.Map<TenantDto>(created);
    }

    public async Task<bool> UpdateAsync(long id, TenantDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await repository.GetByIdAsync(id, cancellationToken);
        if (existing is null) return false;

        mapper.Map(dto, existing);
        await repository.UpdateAsync(existing, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var existing = await repository.GetByIdAsync(id, cancellationToken);
        if (existing is null) return false;

        await repository.DeleteAsync(existing, cancellationToken);
        return true;
    }
}
