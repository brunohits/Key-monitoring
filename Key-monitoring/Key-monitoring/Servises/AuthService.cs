using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Key_monitoring.Interfaces;
using Key_monitoring.DTOs;
using System.Text.RegularExpressions;
using Key_monitoring.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Key_monitoring.Enum;



namespace Key_monitoring.Servises
{ 

public class AuthService : IAuthService
{
    
        private ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public AuthService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
      



        //...........................................<Вход в аккаунт>.......................................................
        public async Task<TokenDTO> Login(UserLoginDTO ForSuccessfulLogin)
        {
            ForSuccessfulLogin.Email = NormalizeAttribute(ForSuccessfulLogin.Email);
            var identity = await GetIdentity(ForSuccessfulLogin.Email, ForSuccessfulLogin.Password);
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: TokenConfigurations.Issuer,
                audience: TokenConfigurations.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.AddMinutes(TokenConfigurations.Lifetime),
                signingCredentials: new SigningCredentials(TokenConfigurations.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));

            var encodeJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

           

            return new TokenDTO()
            {
                Token = encodeJwt
            }; 
        }
     
 

        //..............................<Удаление пробелов и верхнего регистра>............................................

        public static string NormalizeAttribute(string value)
        {
            return value.TrimEnd().ToLower();
        }

      

        //..............................<т наличие пользователя с указанным email в базе данных и после проверка пароля на правильность>....................................................................


        private async Task<ClaimsIdentity> GetIdentity(string email, string password)
        {
            var userEntity = await _dbContext
                .Users
                .FirstOrDefaultAsync(x => x.Email == email);

            if (userEntity == null)
            {
                var exception = new Exception();
                exception.Data.Add(StatusCodes.Status401Unauthorized.ToString(),
                    "User not exists"
                );
                throw exception;
            }

            if (!ValidatePasswordHash(userEntity.Password, password))
            {
                var exception = new Exception();
                exception.Data.Add(StatusCodes.Status401Unauthorized.ToString(),
                    "Wrong password"
                );
                throw exception;
            }

            var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, userEntity.Id.ToString())
        };

            var claimsIdentity = new ClaimsIdentity
            (
                claims,
                "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
            );

            return claimsIdentity;
        }


        //..............................<функция сравнивает хэш введенного пользователем пароля с хэшем, сохраненным в базе данных, для проверки правильности предоставленного пароля>....................................................................

        private static bool ValidatePasswordHash(string savedPasswordHash, string userEnteredPassword)
        {
            var hashBytes = Convert.FromBase64String(savedPasswordHash);
            var storedSalt = new byte[16];
            Array.Copy(hashBytes, 0, storedSalt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(userEnteredPassword, storedSalt, 100000);
            var computedHash = pbkdf2.GetBytes(20);

            for (var i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != computedHash[i])
                {
                    return false;
                }
            }

            return true;
        }

     
    }
}

