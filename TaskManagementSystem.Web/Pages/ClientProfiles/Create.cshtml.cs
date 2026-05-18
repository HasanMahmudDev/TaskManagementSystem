using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagementSystem.Web.Data;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Pages_ClientProfiles
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
        ViewData["UserId"] = new SelectList(_context.UserInfos, "UserId", "Email");
            return Page();
        }

        [BindProperty]
        public ClientProfile ClientProfile { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ClientProfiles.Add(ClientProfile);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
