using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Web.Models;

public class UserInfo
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Role")]
    public int RoleId { get; set; }

    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey(nameof(RoleId))]
    public UserRole Role { get; set; } = null!;

    public ClientProfile? ClientProfile { get; set; }

    public ICollection<TaskCategory> CreatedCategories { get; set; } = new List<TaskCategory>();
    public ICollection<TaskInfo> ClientTasks { get; set; } = new List<TaskInfo>();
    public ICollection<TaskInfo> CreatedTasks { get; set; } = new List<TaskInfo>();
    public ICollection<TaskInfo> ManagedTasks { get; set; } = new List<TaskInfo>();
    public ICollection<TaskUpdate> TaskUpdates { get; set; } = new List<TaskUpdate>();
}
