using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace VTS.Common.Utilities;

public class UserProvider(IHttpContextAccessor httpContextAccessor) : IUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public long? UserId
    {
        get
        {
            if (_httpContextAccessor?.HttpContext?.User == null)
                return null;

            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return long.TryParse(userIdClaim, out long userId) ? userId : null;
        }
    }
}
