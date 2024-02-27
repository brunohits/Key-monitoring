using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.DTOs;

public class KeyStatusDto
{
    [Required(ErrorMessage = "The keyId field is required.")]
    public required Guid KeyId { get; set; }

    public Guid? UserId { get; set; }
}
