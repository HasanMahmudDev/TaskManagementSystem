using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Web.Models;

public class TaskCategory
{
    [Key]
    public int CategoryId { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Category Name")]
    public string CategoryName { get; set; } = string.Empty;

    [Display(Name = "Created By")]
    public int CreatedBy { get; set; }

    [ForeignKey(nameof(CreatedBy))]
    public UserInfo CreatedByUser { get; set; } = null!;

    public ICollection<TaskInfo> Tasks { get; set; } = new List<TaskInfo>();
}
