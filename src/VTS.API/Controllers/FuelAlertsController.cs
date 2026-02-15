using VTS.BLL.DTOs;
using VTS.BLL.Interfaces;
using VTS.API.Controllers.Core;
using Microsoft.AspNetCore.Mvc;

namespace VTS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FuelAlertsController(IFuelAlertService service) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FuelAlertDto dto, CancellationToken cancellationToken)
    {
        var result = await service.Create(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("vehicle/{vehicleId}")]
    public async Task<IActionResult> GetByVehicleId(long vehicleId, CancellationToken cancellationToken)
    {
        var result = await service.GetByVehicleIdAsync(vehicleId, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] FuelAlertDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id)
            return BadRequest("ID mismatch");

        var result = await service.UpdateAsync(dto, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
