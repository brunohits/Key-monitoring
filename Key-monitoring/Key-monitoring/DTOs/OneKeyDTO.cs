using Key_monitoring.Enum;
using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.DTOs;

public class OneKeyDTO
{
    public Guid Id { get; set; }
    public string FacultyName { get; set; }
    public int Number { get; set; }
}

