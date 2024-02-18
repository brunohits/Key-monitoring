using System.ComponentModel.DataAnnotations;


namespace Key_monitoring.Models;

public class TokenModel
{
    [Required]
    public string? InvalidToken { get; set; }
    [Required]
    public DateTime ExpiredDate { get; set; }

}