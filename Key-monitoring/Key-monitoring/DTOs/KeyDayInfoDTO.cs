using Key_monitoring.Enum;
using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.DTOs;

public class KeyDayInfoDTO
{
    public List<KeyStatusEnum> statuses { get; set; }
}
