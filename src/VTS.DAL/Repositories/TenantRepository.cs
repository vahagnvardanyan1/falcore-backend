using VTS.DAL.Entities;
using VTS.DAL.Interfaces;

namespace VTS.DAL.Repositories;

public class TenantRepository(VTSContext db) : ITenantRepository
{
    private readonly VTSContext _db = db;

    public async Task<IEnumerable<Tenant>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _db.Tenants.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<Tenant> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        await _db.Tenants.FindAsync([id], cancellationToken);

    public async Task<Tenant> AddAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        _db.Tenants.Add(tenant);
        await _db.SaveChangesAsync(cancellationToken);
        return tenant;
    }

    public async Task UpdateAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        _db.Tenants.Update(tenant);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        _db.Tenants.Remove(tenant);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
