namespace Key_monitoring.DTOs;

    public class NameAndPaginGetDTO
    {
        public string? Name { get; set; }
        public int? Page { get; set; }
        public int? Size { get; set; }
    }

    public class PaginationDTO
    {
        public int? Size { get; set; }
        public int? Count { get; set; }
        public int? Current { get; set; }
    }

    public class UserNameDTO
    {
        public string Name { get; set; }
    }

    public class FullResponseDTO
    {
        public List<UserNameDTO> Name { get; set; }
        public PaginationDTO Pagination { get; set; }
    }
  
