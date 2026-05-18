using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Web.Data;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Pages_TaskUpdates
{
    public class EditModel : PageModel
    {
        private readonly TaskManagementSystem.Web.Data.AppDbContext _context;

        public EditModel(TaskManagementSystem.Web.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TaskUpdate TaskUpdate { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskupdate =  await _context.TaskUpdates.FirstOrDefaultAsync(m => m.UpdateId == id);
            if (taskupdate == null)
            {
                return NotFound();
            }
            TaskUpdate = taskupdate;
           ViewData["TaskId"] = new SelectList(_context.TaskInfos, "TaskId", "TaskName");
           ViewData["UpdatedBy"] = new SelectList(_context.UserInfos, "UserId", "Email");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TaskUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskUpdateExists(TaskUpdate.UpdateId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TaskUpdateExists(int id)
        {
            return _context.TaskUpdates.Any(e => e.UpdateId == id);
        }
    }
}
