using VTS.BLL.DTOs;
using VTS.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using VTS.API.Controllers.Core;

namespace VTS.API.Controllers;

public class VehiclesController(IVehicleService service) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var items = await service.GetAllAsync(cancellationToken);
        return Ok(items);
    }

    [HttpGet("tenant/{tenantId:int}")]
    public async Task<IActionResult> GetByTenant(long tenantId, CancellationToken cancellationToken)
    {
        var items = await service.GetByTenantAsync(tenantId, cancellationToken);
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
    public async Task<IActionResult> Create([FromBody] VehicleDto dto, CancellationToken cancellationToken)
    {
        var created = await service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(long id, [FromBody] VehicleDto dto, CancellationToken cancellationToken)
    {
        var ok = await service.UpdateAsync(id, dto, cancellationToken);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var ok = await service.DeleteAsync(id, cancellationToken);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpGet("fuel-level/{id:int}")]
    public async Task<IActionResult> GetCurrentFuelLevel(long id, CancellationToken cancellationToken)
    {
        var fuelLevel = await service.GetFuelLevel(id, cancellationToken);
        return Ok(fuelLevel);
    }

    [HttpGet("{id:long}/last-position")]
    public async Task<IActionResult> GetLastPosition(long id, CancellationToken cancellationToken)
    {
        var position = await service.GetLastPositionAsync(id, cancellationToken);
        if (position is null) return NotFound();
        return Ok(position);
    }
}
