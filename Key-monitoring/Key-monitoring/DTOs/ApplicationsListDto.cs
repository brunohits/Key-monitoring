using Key_monitoring.Models;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Key_monitoring.DTOs;

public class ApplicationsListDto
{
    public List<ApplicationsListElementDTO> List { get; set; }

    public PaginationDTO Pagination { get; set; }
}