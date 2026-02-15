using VTS.BLL.DTOs;

namespace VTS.BLL.Interfaces;

public interface ITenantService
{
    Task<IEnumerable<TenantDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TenantDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<TenantDto> CreateAsync(TenantDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, TenantDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
