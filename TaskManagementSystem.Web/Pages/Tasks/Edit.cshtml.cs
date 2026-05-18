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

namespace TaskManagementSystem.Web.Pages_Tasks
{
    public class EditModel : PageModel
    {
        private readonly TaskManagementSystem.Web.Data.AppDbContext _context;

        public EditModel(TaskManagementSystem.Web.Data.AppDbContext context)
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

            var taskinfo =  await _context.TaskInfos.FirstOrDefaultAsync(m => m.TaskId == id);
            if (taskinfo == null)
            {
                return NotFound();
            }
            TaskInfo = taskinfo;
           ViewData["AssignedManagerId"] = new SelectList(_context.UserInfos, "UserId", "Email");
           ViewData["CategoryId"] = new SelectList(_context.TaskCategories, "CategoryId", "CategoryName");
           ViewData["ClientUserId"] = new SelectList(_context.UserInfos, "UserId", "Email");
           ViewData["CreatedBy"] = new SelectList(_context.UserInfos, "UserId", "Email");
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

            _context.Attach(TaskInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskInfoExists(TaskInfo.TaskId))
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

        private bool TaskInfoExists(int id)
        {
            return _context.TaskInfos.Any(e => e.TaskId == id);
        }
    }
}
