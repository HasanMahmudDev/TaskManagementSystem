using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Web.Models;

public class TaskInfo
{
    [Key]
    public int TaskId { get; set; }

    [Required]
    [StringLength(150)]
    [Display(Name = "Task Name")]
    public string TaskName { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [Display(Name = "Start Date")]
    public DateTime? StartDate { get; set; }

    [Display(Name = "End Date")]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Client")]
    public int ClientUserId { get; set; }

    [Display(Name = "Created By")]
    public int CreatedBy { get; set; }

    [Display(Name = "Assigned Manager")]
    public int AssignedManagerId { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public TaskCategory Category { get; set; } = null!;

    [ForeignKey(nameof(ClientUserId))]
    public UserInfo ClientUser { get; set; } = null!;

    [ForeignKey(nameof(CreatedBy))]
    public UserInfo CreatedByUser { get; set; } = null!;

    [ForeignKey(nameof(AssignedManagerId))]
    public UserInfo AssignedManager { get; set; } = null!;

    public ICollection<TaskUpdate> TaskUpdates { get; set; } = new List<TaskUpdate>();
}
