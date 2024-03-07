using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.Models;

public class CodeForEmailModel
{
    [Key]
    public Guid Id { get; set; }
    public int Code { get; set; }
    public Guid IdFromAdress { get; set; }
    public Guid IdToAdress { get; set; }
    public DateTime LifeOfCode { get; set; }
}