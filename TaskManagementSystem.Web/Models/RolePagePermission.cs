using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Web.Models;

public class RolePagePermission
{
    [Key]
    public int PermissionId { get; set; }

    [Display(Name = "Role")]
    public int RoleId { get; set; }

    [Display(Name = "Page")]
    public int PageId { get; set; }

    [Display(Name = "View")]
    public bool CanView { get; set; }

    [Display(Name = "Create")]
    public bool CanCreate { get; set; }

    [Display(Name = "Edit")]
    public bool CanEdit { get; set; }

    [Display(Name = "Delete")]
    public bool CanDelete { get; set; }

    [ForeignKey(nameof(RoleId))]
    public UserRole Role { get; set; } = null!;

    [ForeignKey(nameof(PageId))]
    public AppPage Page { get; set; } = null!;
}
