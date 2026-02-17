using VTS.BLL.DTOs;
using FluentValidation;

namespace VTS.BLL.Validators;

public class VehicleInsuranceDtoValidator : AbstractValidator<VehicleInsuranceDto>
{
    public VehicleInsuranceDtoValidator()
    {
        RuleFor(x => x.VehicleId).GreaterThan(0);
        RuleFor(x => x.Provider).NotEmpty().MaximumLength(150);
        RuleFor(x => x.ExpiryDate).GreaterThan(DateOnly.FromDateTime(DateTime.Today));
    }
}