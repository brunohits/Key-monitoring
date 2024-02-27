using Key_monitoring.DTOs;

namespace Key_monitoring.Interfaces;

public interface IUser
{
    Task<FullResponseDTO> SearchUser(NameAndPaginGetDTO specialityGetDTO);
}