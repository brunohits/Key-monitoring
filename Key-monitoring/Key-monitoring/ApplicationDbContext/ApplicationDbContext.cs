using Microsoft.EntityFrameworkCore;
using Key_monitoring.Models;
namespace Key_monitoring;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<UserModel> Users { get; set; }
    public DbSet<TokenModel> Tokens { get; set; }
    public DbSet<FacultyModel> Faculties { get; set; }
}