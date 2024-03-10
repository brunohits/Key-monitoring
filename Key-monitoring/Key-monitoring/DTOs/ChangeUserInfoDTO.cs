using System.ComponentModel.DataAnnotations;


namespace Key_monitoring.DTOs;

public class ChangeUserInfoDTO
{
    [Required]
    [MinLength(1)]
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public string Phone { get; set; }
    
    public DateTime? BirthDate { get; set; }
}