using Key_monitoring.Models;
using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.DTOs;

public class WeekDTO
{
    public required List<ScheduleModel> pairs { get; set; }
}