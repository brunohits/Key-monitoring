using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.DTOs;

public class KeyCreateDTO
{
    [Required(ErrorMessage = "The CabinetNumber field is required.")]
    public int CabinetNumber { get; set; }

    [StringLength(1000, MinimumLength = 5, ErrorMessage = "The CabinetName must be between 5 and 1000 characters.")]
    public required string CabinetName { get; set; }

    [Required(ErrorMessage = "The FacultyId field is required.")]
    public Guid FacultyId { get; set; }
}