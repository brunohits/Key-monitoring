using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.DTOs;

public class GetKeyDayInfoDTO
{
    [Required(ErrorMessage = "The KeyId field is required.")]
    public required Guid KeyId { get; set; }
    [Required(ErrorMessage = "The day field is required.")]
    public required DateTime day { get; set; }
}