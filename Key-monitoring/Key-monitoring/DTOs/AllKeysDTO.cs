using Key_monitoring.Enum;
using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.DTOs;

public class AllKeysDTO
{
    public List<OneKeyDTO> List { get; set; }
}

