using TaskManagementSystem.Web.Services;

namespace TaskManagementSystem.Web.Middleware;

public class PagePermissionMiddleware
{
    private readonly RequestDelegate _next;

    public PagePermissionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICurrentUserService currentUserService, IPagePermissionService pagePermissionService)
    {
        var path = context.Request.Path.Value ?? "/";

        if (ShouldSkip(path))
        {
            await _next(context);
            return;
        }

        if (context.User.Identity?.IsAuthenticated != true)
        {
            await _next(context);
            return;
        }

        if (!currentUserService.RoleId.HasValue || string.IsNullOrWhiteSpace(currentUserService.RoleName))
        {
            context.Response.Redirect("/Account/AccessDenied");
            return;
        }

        var hasPermission = await pagePermissionService.HasPermissionAsync(
            currentUserService.RoleId.Value,
            currentUserService.RoleName,
            path,
            context.Request.Method);

        if (!hasPermission)
        {
            context.Response.Redirect("/Account/AccessDenied");
            return;
        }

        await _next(context);
    }

    private static bool ShouldSkip(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return true;
        }

        var normalized = path.ToLowerInvariant();

        if (normalized.StartsWith("/account/login") || normalized.StartsWith("/account/logout") || normalized.StartsWith("/account/accessdenied"))
        {
            return true;
        }

        if (normalized.StartsWith("/css/") || normalized.StartsWith("/js/") || normalized.StartsWith("/lib/") || normalized.StartsWith("/favicon"))
        {
            return true;
        }

        return normalized.Contains('.');
    }
}