using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Key_monitoring.Models;

public class KeyFullModelDTO
{
    public DateTime PairStart { get; set; }
    public string status { get; set; }
    public Guid? userId { get; set; }
    public string? userName { get; set; }
}
