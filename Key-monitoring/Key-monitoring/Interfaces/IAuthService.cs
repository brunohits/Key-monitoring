

using Key_monitoring.DTOs;

namespace Key_monitoring.Interfaces;

public interface IAuthService
{
    Task<TokenDTO> Register(UserRegisterDTO userRegisterDTO);
    Task<TokenDTO> Login(UserLoginDTO ForSuccessfulLogin);
    Task Logout(string token);
}