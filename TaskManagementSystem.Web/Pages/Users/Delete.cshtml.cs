using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Web.Data;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Pages_Users
{
    public class DeleteModel : PageModel
    {
        private readonly TaskManagementSystem.Web.Data.AppDbContext _context;

        public DeleteModel(TaskManagementSystem.Web.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UserInfo UserInfo { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userinfo = await _context.UserInfos.FirstOrDefaultAsync(m => m.UserId == id);

            if (userinfo is not null)
            {
                UserInfo = userinfo;

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

            var userinfo = await _context.UserInfos.FindAsync(id);
            if (userinfo != null)
            {
                UserInfo = userinfo;
                _context.UserInfos.Remove(UserInfo);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
