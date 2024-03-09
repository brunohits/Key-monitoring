using System.ComponentModel.DataAnnotations;
using Key_monitoring.Enum;
namespace Key_monitoring.Models;

public class UserModel
{

    public Guid Id { get; set; }
    [Required]
    [MinLength(1)]
    public string? FullName { get; set; }
    public DateTime? BirthDate { get; set; }
    [Required]
    public string? Gender { get; set; }
    [Phone]
    public string? PhoneNumber { get; set; }
    [Required]
    [MinLength(1)]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    [MinLength(6)]
    public string? Password { get; set; }
    public DateTime CreateTime { get; set; }

    public Guid FacultyId { get; set; }
    public RoleEnum Role { get; set; }

}