using Microsoft.EntityFrameworkCore;
using Key_monitoring.Models;
namespace Key_monitoring;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<UserModel> Users { get; set; }
    public DbSet<TokenModel> Tokens { get; set; }
    public DbSet<FacultyModel> Faculties { get; set; }
    public DbSet<CodeForEmailModel> CodeForEmails { get; set; }
    public DbSet<KeyModel> KeyModels { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserModel>().HasKey(x => x.Id);
        modelBuilder.Entity<UserModel>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<UserModel>().HasIndex(u => u.FullName).IsUnique();
    }
}