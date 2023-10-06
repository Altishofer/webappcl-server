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
        }

        modelBuilder.Entity<User>()
            .HasIndex(u => u.userName)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.userId)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(u => u.userName)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.userId)
            .IsRequired();
    }

    public DbSet<User> User { get; set; } = null!;
    public DbSet<TodoItem> TodoItems { get; set; } = null!;
}