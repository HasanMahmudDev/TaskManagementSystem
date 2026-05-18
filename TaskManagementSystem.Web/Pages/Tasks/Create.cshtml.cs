using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagementSystem.Web.Data;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Pages_Tasks
{
    public class CreateModel : PageModel
    {
        private readonly TaskManagementSystem.Web.Data.AppDbContext _context;

        public CreateModel(TaskManagementSystem.Web.Data.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["AssignedManagerId"] = new SelectList(_context.UserInfos, "UserId", "Email");
        ViewData["CategoryId"] = new SelectList(_context.TaskCategories, "CategoryId", "CategoryName");
        ViewData["ClientUserId"] = new SelectList(_context.UserInfos, "UserId", "Email");
        ViewData["CreatedBy"] = new SelectList(_context.UserInfos, "UserId", "Email");
            return Page();
        }

        [BindProperty]
        public TaskInfo TaskInfo { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.TaskInfos.Add(TaskInfo);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
