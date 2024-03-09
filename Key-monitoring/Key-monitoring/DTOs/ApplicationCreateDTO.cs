using Key_monitoring.Models;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Key_monitoring.DTOs;

public class ApplicationCreateDTO
{
    [Required(ErrorMessage = "The pairId field is required.")]
    public required Guid pairId { get; set; }
    [Required(ErrorMessage = "The userId field is required.")]
    public required Guid userId { get; set; }
    [Required(ErrorMessage = "The keyId field is required.")]
    public required Guid keyId { get; set; }
    [Required(ErrorMessage = "The repetitive field is required.")]
    public required bool repetitive { get; set; }

}