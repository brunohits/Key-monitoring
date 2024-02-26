namespace Key_monitoring.DTOs;

public class ListNameDTO
{
    public string Name { get; set; }
}

public class PaginationDTO
{
    public int? Size { get; set; }
    public int? Count { get; set; }
    public int? Page { get; set; }
}

public class NameWithPaginationDTO
{
    public PaginationDTO Pagination { get; set; }
    public List<ListNameDTO> ListName { get; set; } 
}
