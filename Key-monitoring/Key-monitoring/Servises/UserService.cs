using Key_monitoring.DTOs;
using Key_monitoring.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Key_monitoring.Servises;

public class UserService : IUser
{

    private readonly ApplicationDbContext _dbContext;

    public UserService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FullResponseDTO> SearchUser(NameAndPaginGetDTO info)
    {
        const int maxPageSize = 15038;

        if (info.Page == null)
        {
            info.Page = 1;
        }

        if (info.Size == null)
        {
            info.Size = 5;
        }

        if (info.Page <= 0 || info.Size <= 0)
        {
            throw new BadHttpRequestException("Page and Size should be greater than zero.");
        }

        var query = _dbContext.Users
            .Where(u => string.IsNullOrWhiteSpace(info.Name) || u.FullName.Contains(info.Name))
            .Select(u => new UserNameDTO
            {
                Id = u.Id,
                Name = u.FullName,
                Email = u.Email,
                CreateDate = u.CreateTime,
                Role = u.Role,
                Phone = u.PhoneNumber
            });

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((decimal)((double)totalCount / info.Size));

        var userModels = await query
            .Skip((int)((info.Page - 1) * info.Size))
            .Take((int)info.Size)
            .ToListAsync();

        var nameDtosDTO = userModels.Select(s => new UserNameDTO
        {
            Id = s.Id,
            Name = s.Name,
            Email = s.Email,
            CreateDate = s.CreateDate,
            Role = s.Role,
            Phone = s.Phone
        }).ToList();

        var pagination = new PaginationDTO
        {
            Size = info.Size,
            Count = totalPages,
            Current = info.Page
        };

        if (totalCount == 0)
        {
            return new FullResponseDTO
            {
                Name = new List<UserNameDTO>(),
                Pagination = pagination
            };
        }

        return new FullResponseDTO
        {
            Name = nameDtosDTO,
            Pagination = pagination
        };
    }

}
