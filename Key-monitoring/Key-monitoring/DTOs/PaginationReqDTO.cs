using System.ComponentModel.DataAnnotations;

namespace Key_monitoring.DTOs;

public class PaginationReqDTO
{
    [Required(ErrorMessage = "The Page field is required.")]
    public required int Page {  get; set; }

    [Required(ErrorMessage = "The Size field is required.")]
    public required int Size { get; set; }
}