using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.User;

public class UserContext:DbContext
{
    public DbSet<Domain.User> Users { get; set; }
    private readonly ILoggerFactory _loggerFactory;
    public UserContext(DbContextOptions<UserContext> options, ILoggerFactory loggerFactory) 
        : base(options)
    {
        _loggerFactory = loggerFactory;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLoggerFactory(_loggerFactory) // Enable logging
            .EnableSensitiveDataLogging();    // Show sensitive data (e.g., parameter values)
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.User>(builder =>
        {
            builder.ToTable("utilisateur"); 
            builder.HasKey(x => x.id); 
            builder.Property(x => x.id).HasColumnName("id");
            builder.Property(x => x.pseudo).HasColumnName("login").IsRequired();
            builder.Property(x => x.password).HasColumnName("mot_passe");
            builder.Property(x => x.role).HasColumnName("role");
        });
    }
}