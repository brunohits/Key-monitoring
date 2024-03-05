using Key_monitoring.Models;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Key_monitoring.DTOs;

public class ApplicationCreateDTO
{
    public Guid pairId { get; set; }
    public Guid userId { get; set; }
    public Guid keyId { get; set; }
    public bool repetitive { get; set; }

}