using Key_monitoring.DTOs;

namespace Key_monitoring.Interfaces;

public interface IUser
{
    Task<NameWithPaginationDTO> SearchUser(NameWithPaginationDTO name);
}