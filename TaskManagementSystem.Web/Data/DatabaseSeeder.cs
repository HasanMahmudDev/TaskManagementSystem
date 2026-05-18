using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.EnsureCreatedAsync();

        if (!await context.UserRoles.AnyAsync())
        {
            context.UserRoles.AddRange(
                new UserRole { RoleName = "Admin" },
                new UserRole { RoleName = "Employee" },
                new UserRole { RoleName = "Client" },
                new UserRole { RoleName = "Manager" }
            );
            await context.SaveChangesAsync();
        }

        if (!await context.UserInfos.AnyAsync())
        {
            var adminRoleId = await context.UserRoles.Where(x => x.RoleName == "Admin").Select(x => x.RoleId).FirstAsync();
            var employeeRoleId = await context.UserRoles.Where(x => x.RoleName == "Employee").Select(x => x.RoleId).FirstAsync();
            var clientRoleId = await context.UserRoles.Where(x => x.RoleName == "Client").Select(x => x.RoleId).FirstAsync();
            var managerRoleId = await context.UserRoles.Where(x => x.RoleName == "Manager").Select(x => x.RoleId).FirstAsync();

            context.UserInfos.AddRange(
                new UserInfo { FullName = "Hasan Mahmud", Email = "admin@hasan.com", Username = "hasan", Password = "hasan123", RoleId = adminRoleId },
                new UserInfo { FullName = "Hasan Mahmud", Email = "employee@hasan.com", Username = "hasan.emp", Password = "hasan123", RoleId = employeeRoleId },
                new UserInfo { FullName = "Hasan Mahmud", Email = "client@hasan.com", Username = "hasan.client", Password = "hasan123", RoleId = clientRoleId },
                new UserInfo { FullName = "Hasan Mahmud", Email = "manager@hasan.com", Username = "hasan.manager", Password = "hasan123", RoleId = managerRoleId }
            );
            await context.SaveChangesAsync();
        }

        if (!await context.ClientProfiles.AnyAsync())
        {
            var clientUserId = await context.UserInfos
                .Where(x => x.Username == "hasan.client")
                .Select(x => x.UserId)
                .FirstAsync();

            context.ClientProfiles.Add(new ClientProfile
            {
                UserId = clientUserId,
                Company = "Hasan Tech Ltd",
                Contact = "01700000000"
            });

            await context.SaveChangesAsync();
        }

        var requiredPages = new[]
        {
            new AppPage { PageName = "Dashboard", PageUrl = "/Dashboard" },
            new AppPage { PageName = "Users", PageUrl = "/Users" },
            new AppPage { PageName = "Roles", PageUrl = "/Roles" },
            new AppPage { PageName = "App Pages", PageUrl = "/AppPages" },
            new AppPage { PageName = "Role Permissions", PageUrl = "/RolePermissions" },
            new AppPage { PageName = "Client Profiles", PageUrl = "/ClientProfiles" },
            new AppPage { PageName = "Categories", PageUrl = "/Categories" },
            new AppPage { PageName = "Tasks", PageUrl = "/Tasks" },
            new AppPage { PageName = "Task Updates", PageUrl = "/TaskUpdates" },
            new AppPage { PageName = "Permissions", PageUrl = "/Permissions" }
        };

        foreach (var page in requiredPages)
        {
            var exists = await context.AppPages.AnyAsync(x => x.PageUrl == page.PageUrl);
            if (!exists)
            {
                context.AppPages.Add(page);
            }
        }
        await context.SaveChangesAsync();

        var roles = await context.UserRoles.ToDictionaryAsync(x => x.RoleName, x => x.RoleId);
        var pages = await context.AppPages.ToDictionaryAsync(x => x.PageName, x => x.PageId);

        EnsurePermission(context, roles["Admin"], pages["Dashboard"], true, true, true, true);
        EnsurePermission(context, roles["Admin"], pages["Users"], true, true, true, true);
        EnsurePermission(context, roles["Admin"], pages["Roles"], true, true, true, true);
        EnsurePermission(context, roles["Admin"], pages["App Pages"], true, true, true, true);
        EnsurePermission(context, roles["Admin"], pages["Role Permissions"], true, true, true, true);
        EnsurePermission(context, roles["Admin"], pages["Client Profiles"], true, true, true, true);
        EnsurePermission(context, roles["Admin"], pages["Categories"], true, true, true, true);
        EnsurePermission(context, roles["Admin"], pages["Tasks"], true, true, true, true);
        EnsurePermission(context, roles["Admin"], pages["Task Updates"], true, true, true, true);
        EnsurePermission(context, roles["Admin"], pages["Permissions"], true, true, true, true);

        EnsurePermission(context, roles["Manager"], pages["Dashboard"], true, false, false, false);
        EnsurePermission(context, roles["Manager"], pages["Client Profiles"], true, false, true, false);
        EnsurePermission(context, roles["Manager"], pages["Categories"], true, true, true, false);
        EnsurePermission(context, roles["Manager"], pages["Tasks"], true, true, true, false);
        EnsurePermission(context, roles["Manager"], pages["Task Updates"], true, true, true, false);

        EnsurePermission(context, roles["Employee"], pages["Dashboard"], true, false, false, false);
        EnsurePermission(context, roles["Employee"], pages["Tasks"], true, false, false, false);
        EnsurePermission(context, roles["Employee"], pages["Task Updates"], true, true, true, false);

        EnsurePermission(context, roles["Client"], pages["Dashboard"], true, false, false, false);
        EnsurePermission(context, roles["Client"], pages["Tasks"], true, false, false, false);
        EnsurePermission(context, roles["Client"], pages["Client Profiles"], true, false, false, false);

        await context.SaveChangesAsync();

        if (!await context.TaskCategories.AnyAsync())
        {
            var adminId = await context.UserInfos.Where(x => x.Username == "hasan").Select(x => x.UserId).FirstAsync();
            context.TaskCategories.Add(new TaskCategory { CategoryName = "Development", CreatedBy = adminId });
            await context.SaveChangesAsync();
        }

        if (!await context.TaskInfos.AnyAsync())
        {
            var categoryId = await context.TaskCategories.Where(x => x.CategoryName == "Development").Select(x => x.CategoryId).FirstAsync();
            var clientId = await context.UserInfos.Where(x => x.Username == "hasan.client").Select(x => x.UserId).FirstAsync();
            var adminId = await context.UserInfos.Where(x => x.Username == "hasan").Select(x => x.UserId).FirstAsync();
            var managerId = await context.UserInfos.Where(x => x.Username == "hasan.manager").Select(x => x.UserId).FirstAsync();

            context.TaskInfos.Add(new TaskInfo
            {
                TaskName = "RBAC Task Management System",
                Description = "Complete login, role and page permission system",
                CategoryId = categoryId,
                StartDate = new DateTime(2023, 10, 1),
                EndDate = new DateTime(2023, 10, 31),
                ClientUserId = clientId,
                CreatedBy = adminId,
                AssignedManagerId = managerId,
                Status = "In Progress"
            });
            await context.SaveChangesAsync();
        }

        if (!await context.TaskUpdates.AnyAsync())
        {
            var taskId = await context.TaskInfos.Select(x => x.TaskId).FirstAsync();
            var employeeId = await context.UserInfos.Where(x => x.Username == "hasan.emp").Select(x => x.UserId).FirstAsync();

            context.TaskUpdates.Add(new TaskUpdate
            {
                TaskId = taskId,
                UpdatedBy = employeeId,
                UpdateInfo = "Initial system design completed"
            });

            await context.SaveChangesAsync();
        }
    }

    private static void EnsurePermission(
        AppDbContext context,
        int roleId,
        int pageId,
        bool canView,
        bool canCreate,
        bool canEdit,
        bool canDelete)
    {
        var existing = context.RolePagePermissions.FirstOrDefault(x => x.RoleId == roleId && x.PageId == pageId);
        if (existing is not null)
        {
            existing.CanView = canView;
            existing.CanCreate = canCreate;
            existing.CanEdit = canEdit;
            existing.CanDelete = canDelete;
            return;
        }

        context.RolePagePermissions.Add(new RolePagePermission
        {
            RoleId = roleId,
            PageId = pageId,
            CanView = canView,
            CanCreate = canCreate,
            CanEdit = canEdit,
            CanDelete = canDelete
        });
    }
}
