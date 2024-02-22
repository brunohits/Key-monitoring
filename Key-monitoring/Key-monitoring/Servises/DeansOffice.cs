using Key_monitoring.Enum;
using Key_monitoring.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Key_monitoring.Servises;

public class DeansOffice : IDeanOffice
{
    private readonly ApplicationDbContext _dbContext;

    public DeansOffice(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task GiveRole(Guid id, RoleEnum roleEnum, Guid idUser)
    {
        var checkRole = _dbContext.Users.FirstOrDefault(x => x.Id == idUser);
        var checkRoleFor = _dbContext.Users.FirstOrDefault(x => x.Id == id);
        if (checkRole == null)
        {
            var ex = new Exception();
            ex.Data.Add(StatusCodes.Status404NotFound.ToString(), "Данный пользователь не найден");
            throw ex;
        }
        if (id == idUser)
        {
            var ex = new Exception();
            ex.Data.Add(StatusCodes.Status400BadRequest.ToString(), "Вы не можете изменить свою роль");
            throw ex;
        }
        if (checkRoleFor.Role != RoleEnum.DeanOffice)
        {
            var ex = new Exception();
            ex.Data.Add(StatusCodes.Status400BadRequest.ToString(), "Вы не являетесь сотрудником деканата и не имеете права выдавать роли");
            throw ex;
        }
        if (checkRole.Role == roleEnum)
        {
            var ex = new Exception();
            ex.Data.Add(StatusCodes.Status400BadRequest.ToString(), "У данного пользователя такая же роль и была");
            throw ex;
        }

        if (roleEnum != RoleEnum.Student && roleEnum != RoleEnum.Teacher)
        {
            var ex = new Exception();
            ex.Data.Add(StatusCodes.Status400BadRequest.ToString(),"Выберите одну из ролей: Teacher или Student");
            throw ex;
        }

        checkRole.Role = roleEnum;
        await _dbContext.SaveChangesAsync();
    }
}