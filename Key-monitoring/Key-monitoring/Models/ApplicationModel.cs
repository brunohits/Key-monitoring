using Key_monitoring.Enum;
using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.Models;

public class ApplicationModel
{
    [Key]
    public Guid Id { get; set; }

    public required DateTime CreateTime { get; set; }

    public required Guid UserId { get; set; }

    public required UserModel User { get; set; }

    public required Guid ScheduleId { get; set; }

    public required ScheduleModel Schedule { get; set; }

    public required Guid KeyId { get; set; }

    public required ScheduleModel Key { get; set; }

    public required ApplicationStatusEnum status { get; set; }
}