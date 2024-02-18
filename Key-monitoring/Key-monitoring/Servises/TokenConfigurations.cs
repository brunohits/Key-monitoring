using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Key_monitoring.Servises;

internal class TokenConfigurations
{
    public const string Issuer = "Key-Monitoring"; 
    public const string Audience = "StudentAndTeacher";
    private const string Key = "mYGh8lG8d6W7wC1cK2fR3sT4aP5eN9dE0dK8iS6eY3";
    public const int Lifetime = 60;

  
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}