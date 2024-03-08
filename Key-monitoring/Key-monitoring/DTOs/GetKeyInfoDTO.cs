using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.DTOs;

public class GetKeyInfoDTO
{
    [Required(ErrorMessage = "The id field is required.")]
    public required Guid id {  get; set; }

    [Required(ErrorMessage = "The Start field is required.")]
    public required DateTime Start { get; set; }
}