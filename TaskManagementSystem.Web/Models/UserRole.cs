using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Web.Models;

public class UserRole
{
    [Key]
    public int RoleId { get; set; }

    [Required]
    [StringLength(50)]
    public string RoleName { get; set; } = string.Empty;

    public ICollection<UserInfo> Users { get; set; } = new List<UserInfo>();
    public ICollection<RolePagePermission> RolePagePermissions { get; set; } = new List<RolePagePermission>();
}