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
    public class DeleteModel : PageModel
    {
        private readonly TaskManagementSystem.Web.Data.AppDbContext _context;

        public DeleteModel(TaskManagementSystem.Web.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TaskInfo TaskInfo { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskinfo = await _context.TaskInfos.FirstOrDefaultAsync(m => m.TaskId == id);

            if (taskinfo is not null)
            {
                TaskInfo = taskinfo;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskinfo = await _context.TaskInfos.FindAsync(id);
            if (taskinfo != null)
            {
                TaskInfo = taskinfo;
                _context.TaskInfos.Remove(TaskInfo);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
