using VTS.DAL.Entities;
using VTS.DAL.Interfaces;

namespace VTS.DAL.Repositories;

public class VehicleRepository(VTSContext db) : IVehicleRepository
{
    public async Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await db.Vehicles.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<Vehicle> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        await db.Vehicles.FindAsync([id], cancellationToken);

    public async Task<Vehicle> AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        db.Vehicles.Add(vehicle);
        await db.SaveChangesAsync(cancellationToken);
        return vehicle;
    }

    public async Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        db.Vehicles.Update(vehicle);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        db.Vehicles.Remove(vehicle);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Vehicle>> GetByTenantAsync(long tenantId, CancellationToken cancellationToken = default) =>
        await db.Vehicles.AsNoTracking().Where(v => v.TenantId == tenantId).ToListAsync(cancellationToken);

    public async Task<double?> GetFuelLevel(long vehicleId, CancellationToken cancellationToken = default)
    {
        var fuelLevel = await db
            .GpsPositions
            .Where(x => x.VehicleId == vehicleId)
            .OrderByDescending(x => x.TimestampUtc)
            .Select(x => x.FuelLevelLiters)
            .FirstOrDefaultAsync(cancellationToken);

        return fuelLevel;
    }
}
