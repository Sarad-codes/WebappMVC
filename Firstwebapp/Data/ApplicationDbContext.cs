using Firstwebapp.Models;
using Microsoft.EntityFrameworkCore;

namespace Firstwebapp.Data;

public class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    
    public ApplicationDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Users table
        modelBuilder.Entity<UserModel>(entity =>
        {
            // Make Email unique
            entity.HasIndex(e => e.Email)
                .IsUnique();
            
            // Set column types/lengths
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();
                
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsRequired();
                
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsRequired();
                
            entity.Property(e => e.PasswordHash)
                .IsRequired();
                
            // Default value for CreatedAt
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }

    public DbSet<UserModel> Users { get; set; }
}