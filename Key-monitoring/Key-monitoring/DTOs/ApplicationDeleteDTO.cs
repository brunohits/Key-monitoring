using Key_monitoring.Models;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Key_monitoring.DTOs;

public class ApplicationDeleteDTO
{
    [Required(ErrorMessage = "The applicationId field is required.")]
    public Guid applicationId { get; set; }
}