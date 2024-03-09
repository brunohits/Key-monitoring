using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.DTOs;

public class KeyCreateDTO
{
    [Required(ErrorMessage = "The CabinetNumber field is required.")]
    public int CabinetNumber { get; set; }

    [Required(ErrorMessage = "The FacultyId field is required.")]
    public Guid FacultyId { get; set; }
}