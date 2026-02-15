using VTS.BLL.DTOs;
using FluentValidation;

namespace VTS.BLL.Validators;

public class GpsPositionDtoValidator : AbstractValidator<GpsPositionDto>
{
    public GpsPositionDtoValidator()
    {
        RuleFor(x => x.VehicleId).GreaterThan(0);
        RuleFor(x => x.Latitude).InclusiveBetween(-90.0, 90.0);
        RuleFor(x => x.Longitude).InclusiveBetween(-180.0, 180.0);
        RuleFor(x => x.TimestampUtc)
            .Must(dt => dt.Kind == DateTimeKind.Utc)
            .WithMessage("TimestampUtc must be in UTC")
            .Must(dt => dt <= DateTime.UtcNow.AddMinutes(5))
            .WithMessage("TimestampUtc cannot be in the future");

        RuleFor(x => x.SpeedKph).GreaterThanOrEqualTo(0).When(x => x.SpeedKph.HasValue);
        RuleFor(x => x.FuelLevelLiters).GreaterThanOrEqualTo(0).When(x => x.FuelLevelLiters.HasValue);
    }
}
