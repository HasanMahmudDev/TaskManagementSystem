using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Web.Data;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Pages_Roles
{
    public class DetailsModel : PageModel
    {
        private readonly TaskManagementSystem.Web.Data.AppDbContext _context;

        public DetailsModel(TaskManagementSystem.Web.Data.AppDbContext context)
        {
            _context = context;
        }

        public UserRole UserRole { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userrole = await _context.UserRoles.FirstOrDefaultAsync(m => m.RoleId == id);

            if (userrole is not null)
            {
                UserRole = userrole;

                return Page();
            }

            return NotFound();
        }
    }
}
