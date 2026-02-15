using VTS.BLL.DTOs;
using VTS.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using VTS.API.Controllers.Core;

namespace VTS.API.Controllers;

public class GeofencesController(IGeofenceService service) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var items = await service.GetAllAsync(cancellationToken);
        return Ok(items);
    }

    [HttpGet("vehicle/{vehicleId:int}")]
    public async Task<IActionResult> GetByVehicle(long vehicleId, CancellationToken cancellationToken)
    {
        var items = await service.GetByVehicleIdAsync(vehicleId, cancellationToken);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(long id, CancellationToken cancellationToken)
    {
        var item = await service.GetByIdAsync(id, cancellationToken);
        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GeofenceDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await service.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(long id, [FromBody] GeofenceDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var ok = await service.UpdateAsync(id, dto, cancellationToken);
            if (!ok) return NotFound();
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var ok = await service.DeleteAsync(id, cancellationToken);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpGet("contains")]
    public async Task<IActionResult> FindContainingPoint([FromQuery] double latitude, [FromQuery] double longitude, CancellationToken cancellationToken)
    {
        if (double.IsNaN(latitude) || double.IsNaN(longitude))
            return BadRequest(new { error = "Valid latitude and longitude are required." });

        var geofences = await service.FindContainingPointAsync(latitude, longitude, cancellationToken);
        return Ok(geofences);
    }
}
