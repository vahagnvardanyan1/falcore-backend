using VTS.BLL.DTOs;
using VTS.BLL.Interfaces;
using VTS.API.Controllers.Core;
using Microsoft.AspNetCore.Mvc;

namespace VTS.API.Controllers;

public class GpsPositionsController(IGpsPositionService service) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GpsPositionDto dto, CancellationToken cancellationToken)
    {
        await service.Create(dto, cancellationToken);
        return Ok();
    }

    [HttpGet("distance")]
    public async Task<IActionResult> GetDistance(
        long vehicleId,
        DateTime? start,
        DateTime? end,
        CancellationToken cancellationToken)
    {
        var totalMeters = await service.GetDrivenDistance(vehicleId, start, end, cancellationToken);
        return Ok(new { totalMeters });
    }

    [HttpGet("distance-from-last-stop")]
    public async Task<IActionResult> GetDistanceFromLastStop(
        long vehicleId,
        CancellationToken cancellationToken)
    {
        var (lastStopTime, distanceSinceStop) = await service.GetDistanceFromLastStop(vehicleId, cancellationToken);
        return Ok(new { lastStopTime, distanceSinceStop });
    }
}
