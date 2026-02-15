
using FluentValidation;
using VTS.BLL.Validators;
using FluentValidation.AspNetCore;

namespace VTS.API.Extensions;

public static class FluentValidationExtensions
{
    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<TenantDtoValidator>();

        return services;
    }
}
