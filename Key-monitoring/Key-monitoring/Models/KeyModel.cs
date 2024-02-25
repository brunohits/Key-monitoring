using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.Models;

public class KeyModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public DateTime? CreateTime { get; set; }

    [Required]
    public Guid FacultyId { get; set; }

    [Required]
    [MinLength(1)]
    public int CabinetNumber { get; set; }

    [Required]
    [MinLength(1)]
    public string? CabinetName { get; set; }

    public UserModel? Owner { get; set; }
}