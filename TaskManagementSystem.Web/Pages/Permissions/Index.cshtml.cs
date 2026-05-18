using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Web.Data;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Pages.Permissions;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public IList<RolePagePermission> RolePagePermissions { get; set; } = new List<RolePagePermission>();

    public async Task OnGetAsync()
    {
        RolePagePermissions = await _context.RolePagePermissions
            .Include(x => x.Role)
            .Include(x => x.Page)
            .OrderBy(x => x.Role!.RoleName)
            .ThenBy(x => x.Page!.PageName)
            .ToListAsync();
    }
}