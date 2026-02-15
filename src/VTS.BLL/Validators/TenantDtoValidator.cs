using VTS.BLL.DTOs;
using FluentValidation;

namespace VTS.BLL.Validators;

public class TenantDtoValidator : AbstractValidator<TenantDto>
{
    public TenantDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Slug).NotEmpty().MaximumLength(100);
        RuleFor(x => x.APIKey).NotEmpty();
    }
}
