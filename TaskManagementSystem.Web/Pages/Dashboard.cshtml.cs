using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Web.Data;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Pages;

public class DashboardModel : PageModel
{
    private readonly AppDbContext _context;

    public DashboardModel(AppDbContext context)
    {
        _context = context;
    }

    public int UserCount { get; set; }
    public int TaskCount { get; set; }
    public int CategoryCount { get; set; }
    public int TaskUpdateCount { get; set; }
    public List<TaskInfo> RecentTasks { get; set; } = new();

    public async Task OnGetAsync()
    {
        UserCount = await _context.UserInfos.CountAsync();
        TaskCount = await _context.TaskInfos.CountAsync();
        CategoryCount = await _context.TaskCategories.CountAsync();
        TaskUpdateCount = await _context.TaskUpdates.CountAsync();

        RecentTasks = await _context.TaskInfos
            .Include(x => x.Category)
            .Include(x => x.ClientUser)
            .OrderByDescending(x => x.TaskId)
            .Take(5)
            .ToListAsync();
    }
}