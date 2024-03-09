using Key_monitoring.Enum;
using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.DTOs;

public class IdListDTO
{
    public List<Guid> idList { get; set; }
}
