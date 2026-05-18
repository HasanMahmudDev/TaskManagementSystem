using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Web.Data;

namespace TaskManagementSystem.Web.Services;

public interface IPagePermissionService
{
    Task<bool> HasPermissionAsync(int roleId, string roleName, string path, string method);
}

public class PagePermissionService : IPagePermissionService
{
    private readonly AppDbContext _context;

    public PagePermissionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> HasPermissionAsync(int roleId, string roleName, string path, string method)
    {
        if (string.Equals(roleName, "Admin", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var normalizedPath = NormalizePath(path);
        var normalizedMethod = method.ToUpperInvariant();

        var pages = await _context.AppPages.AsNoTracking().ToListAsync();
        var matchingPage = pages
            .OrderByDescending(x => x.PageUrl.Length)
            .FirstOrDefault(x =>
            {
                var url = NormalizePath(x.PageUrl);
                return normalizedPath == url || normalizedPath.StartsWith(url + "/", StringComparison.OrdinalIgnoreCase);
            });

        if (matchingPage is null)
        {
            return true;
        }

        var permission = await _context.RolePagePermissions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.RoleId == roleId && x.PageId == matchingPage.PageId);

        if (permission is null)
        {
            return false;
        }

        var operation = DetectOperation(normalizedPath, normalizedMethod);

        return operation switch
        {
            "create" => permission.CanCreate,
            "edit" => permission.CanEdit,
            "delete" => permission.CanDelete,
            _ => permission.CanView
        };
    }

    private static string NormalizePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return "/";
        }

        var normalized = path.Trim();
        if (!normalized.StartsWith('/'))
        {
            normalized = "/" + normalized;
        }

        normalized = normalized.TrimEnd('/');
        return string.IsNullOrWhiteSpace(normalized) ? "/" : normalized;
    }

    private static string DetectOperation(string path, string method)
    {
        if (path.Contains("/create", StringComparison.OrdinalIgnoreCase))
        {
            return "create";
        }

        if (path.Contains("/edit", StringComparison.OrdinalIgnoreCase))
        {
            return "edit";
        }

        if (path.Contains("/delete", StringComparison.OrdinalIgnoreCase))
        {
            return "delete";
        }

        if (method == "POST")
        {
            return "edit";
        }

        return "view";
    }
}