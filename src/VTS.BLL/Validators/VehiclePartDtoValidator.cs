using VTS.BLL.DTOs;
using FluentValidation;

namespace VTS.BLL.Validators;

public class VehiclePartDtoValidator : AbstractValidator<VehiclePartDto>
{
    public VehiclePartDtoValidator()
    {
        RuleFor(x => x.VehicleId)
            .GreaterThan(0)
            .WithMessage("VehicleId must be greater than 0");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(200)
            .WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.PartNumber)
            .NotEmpty()
            .WithMessage("PartNumber is required")
            .MaximumLength(100)
            .WithMessage("PartNumber cannot exceed 100 characters");

        RuleFor(x => x.ServiceIntervalKm)
            .GreaterThan(0)
            .WithMessage("ServiceIntervalKm must be greater than 0");

        RuleFor(x => x.LastServiceOdometerKm)
            .GreaterThanOrEqualTo(0)
            .WithMessage("LastServiceOdometerKm cannot be negative");

        RuleFor(x => x.NextServiceOdometerKm)
            .GreaterThan(x => x.LastServiceOdometerKm)
            .WithMessage("NextServiceOdometerKm must be greater than LastServiceOdometerKm");
    }
}
