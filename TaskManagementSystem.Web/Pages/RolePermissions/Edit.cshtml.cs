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

namespace TaskManagementSystem.Web.Pages_RolePermissions
{
    public class EditModel : PageModel
    {
        private readonly TaskManagementSystem.Web.Data.AppDbContext _context;

        public EditModel(TaskManagementSystem.Web.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RolePagePermission RolePagePermission { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rolepagepermission =  await _context.RolePagePermissions.FirstOrDefaultAsync(m => m.PermissionId == id);
            if (rolepagepermission == null)
            {
                return NotFound();
            }
            RolePagePermission = rolepagepermission;
           ViewData["PageId"] = new SelectList(_context.AppPages, "PageId", "PageName");
           ViewData["RoleId"] = new SelectList(_context.UserRoles, "RoleId", "RoleName");
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

            _context.Attach(RolePagePermission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolePagePermissionExists(RolePagePermission.PermissionId))
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

        private bool RolePagePermissionExists(int id)
        {
            return _context.RolePagePermissions.Any(e => e.PermissionId == id);
        }
    }
}
