using Microsoft.EntityFrameworkCore;
using Key_monitoring.Models;
namespace Key_monitoring;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<UserModel> Users { get; set; }
    public DbSet<TokenModel> Tokens { get; set; }
    public DbSet<FacultyModel> Faculties { get; set; }
    public DbSet<KeyModel> Keys { get; set; }
    public DbSet<ScheduleModel> Schedule {  get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserModel>().HasKey(x => x.Id);
        modelBuilder.Entity<UserModel>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<UserModel>().HasIndex(u => u.FullName).IsUnique();
    }
}