using Key_monitoring.Enum;
using Key_monitoring.Models;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Key_monitoring.DTOs;

public class ApplicationsListUserDTO
{
    public Guid userId { get; set; }
    public ApplicationStatusEnum? status { get; set; }
}