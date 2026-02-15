using VTS.BLL.DTOs;
using FluentValidation;

namespace VTS.BLL.Validators;

public class VehicleTechnicalInspectionDtoValidator : AbstractValidator<VehicleTechnicalInspectionDto>
{
    public VehicleTechnicalInspectionDtoValidator()
    {
        RuleFor(x => x.VehicleId)
            .GreaterThan(0)
            .WithMessage("VehicleId must be greater than 0");

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("ExpiryDate must be in the future");
    }
}
