using Microsoft.EntityFrameworkCore;

namespace ToX.Models;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName().ToLower());

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName().ToLower());
            }
        }
    }
    
    public DbSet<Host> Host { get; set; } = null!;
    public DbSet<WordVector> WordVector { get; set; } = null!;
    public DbSet<Quiz> Quiz { get; set; } = null!;    
    public DbSet<Answer> Answer { get; set; } = null!;
    public DbSet<Round> Round { get; set; } = null!;
    public DbSet<Player> Player { get; set; } = null!;

}