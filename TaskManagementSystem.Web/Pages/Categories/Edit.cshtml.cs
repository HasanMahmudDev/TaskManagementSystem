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

namespace TaskManagementSystem.Web.Pages_Categories
{
    public class EditModel : PageModel
    {
        private readonly TaskManagementSystem.Web.Data.AppDbContext _context;

        public EditModel(TaskManagementSystem.Web.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TaskCategory TaskCategory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskcategory =  await _context.TaskCategories.FirstOrDefaultAsync(m => m.CategoryId == id);
            if (taskcategory == null)
            {
                return NotFound();
            }
            TaskCategory = taskcategory;
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

            _context.Attach(TaskCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskCategoryExists(TaskCategory.CategoryId))
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

        private bool TaskCategoryExists(int id)
        {
            return _context.TaskCategories.Any(e => e.CategoryId == id);
        }
    }
}
