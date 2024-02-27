using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.Models;

public class ScheduleModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public DateTime PairStart { get; set; }
}
