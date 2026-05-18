using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<UserInfo> UserInfos => Set<UserInfo>();
    public DbSet<ClientProfile> ClientProfiles => Set<ClientProfile>();
    public DbSet<AppPage> AppPages => Set<AppPage>();
    public DbSet<RolePagePermission> RolePagePermissions => Set<RolePagePermission>();
    public DbSet<TaskCategory> TaskCategories => Set<TaskCategory>();
    public DbSet<TaskInfo> TaskInfos => Set<TaskInfo>();
    public DbSet<TaskUpdate> TaskUpdates => Set<TaskUpdate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRole");
            entity.HasKey(x => x.RoleId);
            entity.HasIndex(x => x.RoleName).IsUnique();
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.ToTable("UserInfo");
            entity.HasKey(x => x.UserId);
            entity.HasIndex(x => x.Email).IsUnique();
            entity.HasIndex(x => x.Username).IsUnique();

            entity.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            entity.HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ClientProfile>(entity =>
        {
            entity.ToTable("ClientProfile");
            entity.HasKey(x => x.ClientId);
            entity.HasIndex(x => x.UserId).IsUnique();

            entity.HasOne(x => x.User)
                .WithOne(x => x.ClientProfile)
                .HasForeignKey<ClientProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AppPage>(entity =>
        {
            entity.ToTable("AppPage");
            entity.HasKey(x => x.PageId);
            entity.HasIndex(x => x.PageName).IsUnique();
            entity.HasIndex(x => x.PageUrl).IsUnique();
        });

        modelBuilder.Entity<RolePagePermission>(entity =>
        {
            entity.ToTable("RolePagePermission");
            entity.HasKey(x => x.PermissionId);
            entity.HasIndex(x => new { x.RoleId, x.PageId }).IsUnique();

            entity.HasOne(x => x.Role)
                .WithMany(x => x.RolePagePermissions)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Page)
                .WithMany(x => x.RolePagePermissions)
                .HasForeignKey(x => x.PageId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TaskCategory>(entity =>
        {
            entity.ToTable("TaskCategory");
            entity.HasKey(x => x.CategoryId);

            entity.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.CreatedCategories)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TaskInfo>(entity =>
        {
            entity.ToTable("TaskInfo");
            entity.HasKey(x => x.TaskId);

            entity.HasOne(x => x.Category)
                .WithMany(x => x.Tasks)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.ClientUser)
                .WithMany(x => x.ClientTasks)
                .HasForeignKey(x => x.ClientUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.CreatedTasks)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.AssignedManager)
                .WithMany(x => x.ManagedTasks)
                .HasForeignKey(x => x.AssignedManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TaskUpdate>(entity =>
        {
            entity.ToTable("TaskUpdate");
            entity.HasKey(x => x.UpdateId);

            entity.Property(x => x.UpdatedAt)
                .HasDefaultValueSql("GETDATE()");

            entity.HasOne(x => x.Task)
                .WithMany(x => x.TaskUpdates)
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.UpdatedByUser)
                .WithMany(x => x.TaskUpdates)
                .HasForeignKey(x => x.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}