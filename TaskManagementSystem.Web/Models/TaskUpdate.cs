using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Web.Models;

public class TaskUpdate
{
    [Key]
    public int UpdateId { get; set; }

    [Display(Name = "Task")]
    public int TaskId { get; set; }

    [Display(Name = "Updated By")]
    public int UpdatedBy { get; set; }

    [Required]
    [Display(Name = "Update Information")]
    public string UpdateInfo { get; set; } = string.Empty;

    [Display(Name = "Updated At")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [ForeignKey(nameof(TaskId))]
    public TaskInfo Task { get; set; } = null!;

    [ForeignKey(nameof(UpdatedBy))]
    public UserInfo UpdatedByUser { get; set; } = null!;
}
