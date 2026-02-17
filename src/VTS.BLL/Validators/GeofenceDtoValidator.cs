using VTS.BLL.DTOs;
using FluentValidation;

namespace VTS.BLL.Validators;

public class GeofenceDtoValidator : AbstractValidator<GeofenceDto>
{
    public GeofenceDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Geofence name is required.")
            .MaximumLength(255).WithMessage("Geofence name must not exceed 255 characters.");

        RuleFor(x => x.VehicleId)
            .GreaterThan(0).WithMessage("Valid VehicleId is required.");

        RuleFor(x => x.Center)
            .NotNull().WithMessage("Center coordinates are required.")
            .Must(c => !double.IsNaN(c.Latitude) && !double.IsNaN(c.Longitude))
            .WithMessage("Valid center coordinates are required.");

        RuleFor(x => x.RadiusMeters)
            .GreaterThan(0).WithMessage("RadiusMeters must be greater than 0.");
    }
}
