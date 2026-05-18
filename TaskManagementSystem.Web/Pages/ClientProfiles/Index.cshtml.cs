using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Web.Data;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Pages_ClientProfiles
{
    public class IndexModel : PageModel
    {
        private readonly TaskManagementSystem.Web.Data.AppDbContext _context;

        public IndexModel(TaskManagementSystem.Web.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<ClientProfile> ClientProfile { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ClientProfile = await _context.ClientProfiles
                .Include(c => c.User).ToListAsync();
        }
    }
}
