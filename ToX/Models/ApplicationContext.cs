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

        modelBuilder.Entity<Host>()
            .HasIndex(u => u.hostName)
            .IsUnique();

        modelBuilder.Entity<Host>()
            .HasIndex(u => u.hostId)
            .IsUnique();

        modelBuilder.Entity<Host>()
            .Property(u => u.hostName)
            .IsRequired();

        modelBuilder.Entity<Host>()
            .Property(u => u.hostId)
            .IsRequired();
        
        modelBuilder.Entity<Player>()
            .HasIndex(u => u.PlayerName)
            .IsUnique();

        modelBuilder.Entity<Player>()
            .HasIndex(u => u.Id)
            .IsUnique();

        modelBuilder.Entity<Player>()
            .Property(u => u.PlayerName)
            .IsRequired();

        modelBuilder.Entity<Player>()
            .Property(u => u.Id)
            .IsRequired();
    }

    public DbSet<Host> Host { get; set; } = null!;
    public DbSet<TodoItem> TodoItems { get; set; } = null!;
    public DbSet<WordVector> WordVector { get; set; } = null!;
    
    public DbSet<Quiz> Quiz { get; set; } = null!;    
    public DbSet<Answer> Answer { get; set; } = null!;
    public DbSet<Round> Round { get; set; } = null!;
    public DbSet<Player> Player { get; set; } = null!;

}