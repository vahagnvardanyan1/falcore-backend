using VTS.BLL.DTOs;
using FluentValidation;

namespace VTS.BLL.Validators;

public class VehicleDtoValidator : AbstractValidator<VehicleDto>
{
    public VehicleDtoValidator()
    {
        RuleFor(x => x.PlateNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.VIN).MaximumLength(50).When(x => !string.IsNullOrWhiteSpace(x.VIN));
        RuleFor(x => x.Make).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Model).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Year).InclusiveBetween(1886, DateTime.UtcNow.Year + 1);
        RuleFor(x => x.TotalMileage).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TenantId).GreaterThan(0);
    }
}
