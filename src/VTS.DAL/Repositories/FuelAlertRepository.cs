using VTS.DAL.Entities;
using VTS.DAL.Interfaces;

namespace VTS.DAL.Repositories;

public class FuelAlertRepository(VTSContext dbContext) : IFuelAlertRepository
{
    public async Task<FuelAlert> Create(FuelAlert fuelAlert, CancellationToken cancellationToken)
    {
        dbContext.FuelAlerts.Add(fuelAlert);
        await dbContext.SaveChangesAsync(cancellationToken);
        return fuelAlert;
    }

    public async Task<FuelAlert> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await dbContext.FuelAlerts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<FuelAlert>> GetByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default)
    {
        return await dbContext.FuelAlerts
            .AsNoTracking()
            .Where(x => x.VehicleId == vehicleId)
            .OrderByDescending(x => x.CreatedDateUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<FuelAlert> UpdateAsync(FuelAlert fuelAlert, CancellationToken cancellationToken)
    {
        dbContext.FuelAlerts.Update(fuelAlert);
        await dbContext.SaveChangesAsync(cancellationToken);
        return fuelAlert;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var fuelAlert = await dbContext.FuelAlerts.FindAsync([id], cancellationToken: cancellationToken);
        if (fuelAlert != null)
        {
            dbContext.FuelAlerts.Remove(fuelAlert);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
