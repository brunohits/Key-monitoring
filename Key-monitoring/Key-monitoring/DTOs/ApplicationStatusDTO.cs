using Key_monitoring.Enum;
using System.ComponentModel.DataAnnotations;


namespace Key_monitoring.DTOs;

public class ApplicationStatusDTO
{
    [Required(ErrorMessage = "The id field is required.")]
    public required Guid id {  get; set; }
    [Required(ErrorMessage = "The status field is required.")]
    public required ApplicationStatusEnum status { get; set; }
}
