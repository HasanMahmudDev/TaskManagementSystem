using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Web.Data;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Pages_Tasks
{
    public class IndexModel : PageModel
    {
        private readonly TaskManagementSystem.Web.Data.AppDbContext _context;

        public IndexModel(TaskManagementSystem.Web.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<TaskInfo> TaskInfo { get;set; } = default!;

        public async Task OnGetAsync()
        {
            TaskInfo = await _context.TaskInfos
                .Include(t => t.AssignedManager)
                .Include(t => t.Category)
                .Include(t => t.ClientUser)
                .Include(t => t.CreatedByUser).ToListAsync();
        }
    }
}
