using System.Security.Claims;

namespace TaskManagementSystem.Web.Services;

public interface ICurrentUserService
{
    int? UserId { get; }
    int? RoleId { get; }
    string? RoleName { get; }
    string? Username { get; }
}

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UserId => TryGetIntClaim(ClaimTypes.NameIdentifier);
    public int? RoleId => TryGetIntClaim("RoleId");
    public string? RoleName => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
    public string? Username => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);

    private int? TryGetIntClaim(string claimType)
    {
        var value = _httpContextAccessor.HttpContext?.User.FindFirstValue(claimType);
        return int.TryParse(value, out var parsed) ? parsed : null;
    }
}