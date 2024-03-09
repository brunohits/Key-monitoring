using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.DTOs;

public class GetWeekDTO
{
    [Required(ErrorMessage = "The Start field is required.")]
    public required DateTime WeekStart { get; set; }
}