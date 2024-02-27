using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.DTOs;

public class DateDTO
{
    [Required(ErrorMessage = "The Start field is required.")]
    public required DateTime Start { get; set; }

    [Required(ErrorMessage = "The Finish field is required.")]
    public required DateTime Finish { get; set; }
}