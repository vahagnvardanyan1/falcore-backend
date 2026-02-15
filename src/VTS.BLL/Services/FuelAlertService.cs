using AutoMapper;
using VTS.BLL.DTOs;
using VTS.DAL.Entities;
using VTS.BLL.Exceptions;
using VTS.BLL.Interfaces;
using VTS.DAL.Interfaces;

namespace VTS.BLL.Services;

public class FuelAlertService(
    IFuelAlertRepository fuelAlertRepository,
    IVehicleRepository vehicleRepository,
    IMapper mapper) : IFuelAlertService
{
    public async Task<FuelAlertDto> Create(FuelAlertDto fuelAlertDto, CancellationToken cancellationToken)
    {
        var vehicle = await vehicleRepository.GetByIdAsync(fuelAlertDto.VehicleId, cancellationToken)
            ?? throw new NotFoundException($"Vehicle with {fuelAlertDto.VehicleId} was not found");

        var fuelAlert = mapper.Map<FuelAlert>(fuelAlertDto);
        await fuelAlertRepository.Create(fuelAlert, cancellationToken);

        return mapper.Map<FuelAlertDto>(fuelAlert);
    }

    public async Task<FuelAlertDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var fuelAlert = await fuelAlertRepository.GetByIdAsync(id, cancellationToken);
        return fuelAlert == null ? null : mapper.Map<FuelAlertDto>(fuelAlert);
    }

    public async Task<IEnumerable<FuelAlertDto>> GetByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken = default)
    {
        var fuelAlerts = await fuelAlertRepository.GetByVehicleIdAsync(vehicleId, cancellationToken);
        return fuelAlerts.Select(fa => mapper.Map<FuelAlertDto>(fa)).ToList();
    }

    public async Task<FuelAlertDto> UpdateAsync(FuelAlertDto fuelAlertDto, CancellationToken cancellationToken)
    {
        var existingAlert = await fuelAlertRepository.GetByIdAsync(fuelAlertDto.Id, cancellationToken)
            ?? throw new NotFoundException($"Fuel alert with {fuelAlertDto.Id} was not found");

        var fuelAlert = mapper.Map<FuelAlert>(fuelAlertDto);
        await fuelAlertRepository.UpdateAsync(fuelAlert, cancellationToken);

        return mapper.Map<FuelAlertDto>(fuelAlert);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var fuelAlert = await fuelAlertRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Fuel alert with {id} was not found");

        await fuelAlertRepository.DeleteAsync(id, cancellationToken);
    }
}
