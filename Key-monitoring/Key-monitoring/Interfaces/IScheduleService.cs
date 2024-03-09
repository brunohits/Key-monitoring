using Key_monitoring.DTOs;
using Key_monitoring.Enum;
using Key_monitoring.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Key_monitoring.Interfaces;

public interface IScheduleService
{
    Task<IdListDTO> CreateWeek(DateTime start);
    Task<WeekDTO> GetWeek(DateTime start);
}