using VTS.DAL.Entities;
using VTS.DAL.Extensions;
using VTS.DAL.Interfaces;

namespace VTS.DAL.Repositories;

public class GpsPositionRepository(VTSContext dbContext) : IGpsPositionRepository
{
    public async Task<GpsPosition> Create(GpsPosition position, CancellationToken cancellationToken)
    {
        dbContext.GpsPositions.Add(position);
        await dbContext.SaveChangesAsync(cancellationToken);
        return position;
    }

    public async Task<GpsPosition> GetLastPositionByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default)
    {
        return await dbContext.GpsPositions
            .AsNoTracking()
            .Where(gp => gp.VehicleId == vehicleId)
            .OrderByDescending(gp => gp.TimestampUtc)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
