using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.User;

public class UserContext:DbContext
{
    public DbSet<Domain.User> Users { get; set; }
    public DbSet<Domain.Mocktail> Mocktails { get; set; }
    public DbSet<Domain.Ingredient> Ingredients { get; set; }
    public DbSet<Domain.MocktailIngredient> MocktailIngredients { get; set; }
    
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
            builder.ToTable("user_account"); 
            builder.HasKey(x => x.id); 
            builder.Property(x => x.id).HasColumnName("id");
            builder.Property(x => x.pseudo).HasColumnName("username").IsRequired();
            builder.Property(x => x.password).HasColumnName("password");
            builder.Property(x => x.role).HasColumnName("role");
        });

        modelBuilder.Entity<Domain.Mocktail>(builder =>
        {
            builder.ToTable("mocktail");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Name).HasColumnName("name").IsRequired();
            builder.Property(x => x.Description).HasColumnName("description");
            builder.Property(x => x.Price).HasColumnName("price").HasColumnType("decimal(10,2)");
            builder.Property(x => x.Image).HasColumnName("image"); //Need explication
        });

        modelBuilder.Entity<Domain.Ingredient>(builder =>
        {
            builder.ToTable("ingredient");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Name).HasColumnName("name").IsRequired();
            builder.Property(x => x.Stock).HasColumnName("quantity").HasColumnType("decimal(10,2)");
            builder.Property(x => x.Limit).HasColumnName("restock_threshold").HasColumnType("decimal(10,2)");
            builder.Property(x => x.Unit).HasColumnName("unit").HasMaxLength(10);
        });

        modelBuilder.Entity<Domain.MocktailIngredient>(builder =>
        {
            builder.ToTable("mocktail_ingredient");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.MocktailId).HasColumnName("mocktail_id");
            builder.Property(x => x.IngredientId).HasColumnName("ingredient_id");
            builder.Property(x => x.Quantity).HasColumnName("quantity").HasColumnType("decimal(10,2)");
            builder.Property(x => x.Unit).HasColumnName("unit").HasMaxLength(10);

            // Relations
            builder.HasOne(x => x.Mocktail)
                .WithMany(x => x.MocktailIngredients)
                .HasForeignKey(x => x.MocktailId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Ingredient)
                .WithMany(x => x.MocktailIngredients)
                .HasForeignKey(x => x.IngredientId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}