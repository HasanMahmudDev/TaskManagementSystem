using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Web.Models;

public class AppPage
{
    [Key]
    public int PageId { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Page Name")]
    public string PageName { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    [Display(Name = "Page URL")]
    public string PageUrl { get; set; } = string.Empty;

    public ICollection<RolePagePermission> RolePagePermissions { get; set; } = new List<RolePagePermission>();
}