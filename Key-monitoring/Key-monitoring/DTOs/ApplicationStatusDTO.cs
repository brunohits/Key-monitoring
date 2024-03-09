using Key_monitoring.Enum;
using System.ComponentModel.DataAnnotations;


namespace Key_monitoring.DTOs;

public class ApplicationStatusDTO
{
    public required Guid id {  get; set; }
    public required ApplicationStatusEnum status { get; set; }
}
