using VTS.BLL.DTOs;
using FluentValidation;

namespace VTS.BLL.Validators;

public class FuelAlertDtoValidator : AbstractValidator<FuelAlertDto>
{
    public FuelAlertDtoValidator()
    {
        RuleFor(x => x.VehicleId)
            .GreaterThan(0)
            .WithMessage("VehicleId must be greater than 0");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(150)
            .WithMessage("Name must not exceed 150 characters");

        RuleFor(x => x.ThresholdValue)
            .GreaterThanOrEqualTo(0)
            .WithMessage("ThresholdValue must be greater than or equal to 0");

        RuleFor(x => x.AlertType)
            .IsInEnum()
            .WithMessage("AlertType must be either Low (0) or High (1)");
    }
}
