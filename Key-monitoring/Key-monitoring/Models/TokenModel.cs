using System.ComponentModel.DataAnnotations;


namespace Key_monitoring.Models;

public class TokenModel
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string? InvalidToken { get; set; }
    [Required]
    public DateTime ExpiredDate { get; set; }

}