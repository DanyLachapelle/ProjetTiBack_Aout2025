using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.User;

public class DbContext:Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Domain.UserAccount> Users { get; set; }
    public DbSet<Domain.mocktail> Mocktails { get; set; }
    public DbSet<Domain.ingredient> Ingredients { get; set; }
    public DbSet<Domain.MocktailIngredient> MocktailIngredients { get; set; }
    
    private readonly ILoggerFactory _loggerFactory;
    public DbContext(DbContextOptions<DbContext> options, ILoggerFactory loggerFactory) 
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
        modelBuilder.Entity<Domain.UserAccount>(builder =>
        {
            builder.ToTable("user_account"); 
            builder.HasKey(x => x.id); 
            builder.Property(x => x.id).HasColumnName("id");
            builder.Property(x => x.username).HasColumnName("username").IsRequired();
            builder.Property(x => x.password).HasColumnName("password").IsRequired();
            builder.Property(x => x.role).HasColumnName("role");
        });

        modelBuilder.Entity<Domain.mocktail>(builder =>
        {
            builder.ToTable("mocktail");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Name).HasColumnName("nom").IsRequired();
            builder.Property(x => x.Description).HasColumnName("description");
            builder.Property(x => x.Price).HasColumnName("prix").HasColumnType("decimal(10,2)");
            builder.Property(x => x.Image).HasColumnName("image");
        });

        modelBuilder.Entity<Domain.ingredient>(builder =>
        {
            builder.ToTable("ingredient");
            builder.HasKey(x => x.id);
            builder.Property(x => x.id).HasColumnName("id");
            builder.Property(x => x.name).HasColumnName("name").IsRequired();
            builder.Property(x => x.quantity).HasColumnName("quantity").HasColumnType("decimal(10,2)");
            builder.Property(x => x.restock_threshold).HasColumnName("restock_threshold").HasColumnType("decimal(10,2)");
            builder.Property(x => x.unit).HasColumnName("unit").HasMaxLength(10);
            builder.Property(x => x.last_modified_at).HasColumnName("last_modified_at").HasColumnType("DATETIME2");
        });

        modelBuilder.Entity<Domain.MocktailIngredient>(builder =>
        {
            builder.ToTable("mocktail_ingredient");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.MocktailId).HasColumnName("mocktail_id");
            builder.Property(x => x.IngredientId).HasColumnName("ingredient_id");
            builder.Property(x => x.Quantity).HasColumnName("quantite").HasColumnType("decimal(10,2)");
            builder.Property(x => x.Unit).HasColumnName("unite").HasMaxLength(10);

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