using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Web.Models;

public class ClientProfile
{
    [Key]
    public int ClientId { get; set; }

    [Display(Name = "Client User")]
    public int UserId { get; set; }

    [StringLength(100)]
    public string? Company { get; set; }

    [StringLength(50)]
    public string? Contact { get; set; }

    [ForeignKey(nameof(UserId))]
    public UserInfo User { get; set; } = null!;
}
