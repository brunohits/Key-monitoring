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
    public async Task<NameWithPaginationDTO> SearchUser(NameWithPaginationDTO nameWithPagination)
    {
        
        const int maxPageSize = 15038;
        if (nameWithPagination.Pagination.Page == null)
        {
            nameWithPagination.Pagination.Page = 1;
        }
        if (nameWithPagination.Pagination.Size == null)
        {
            nameWithPagination.Pagination.Size = 5;
        }

        if (nameWithPagination.Pagination.Page <= 0 || nameWithPagination.Pagination.Size <= 0)
        {
            throw new BadHttpRequestException("Page and Size should be greater than zero.");
        }
        var query = _dbContext.Users.AsQueryable();
        if (nameWithPagination.ListName != null && nameWithPagination.ListName.Any())
        {
            query = query.Where(s => s.FullName.ToUpper().Contains(nameWithPagination.ListName.First().Name.ToUpper()));
        }


        var totalCount = await query.CountAsync();

        if (nameWithPagination.Pagination.Size > maxPageSize || nameWithPagination.Pagination.Page > (totalCount + nameWithPagination.Pagination.Size - 1) / nameWithPagination.Pagination.Size)
        {
            throw new BadHttpRequestException("Invalid Page or Size value.");
        }

        var names = await query
            .Skip((int)((nameWithPagination.Pagination.Page - 1) * nameWithPagination.Pagination.Size))
            .Take((int)nameWithPagination.Pagination.Size)
            .ToListAsync();

        var Name = names.Select(s => new ListNameDTO
        {
            Name = s.FullName,
        }).ToList();

        var pagination = new PaginationDTO
        {
            Size = nameWithPagination.Pagination.Size,
            Count = totalCount,
            Page = nameWithPagination.Pagination.Page
        };

        return new NameWithPaginationDTO
        {
            ListName = Name,
            Pagination = pagination
        };
    }
}