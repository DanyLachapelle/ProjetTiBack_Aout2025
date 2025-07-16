using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.User;

public class DbContext:Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Domain.UserAccount> Users { get; set; }
    public DbSet<Domain.mocktail> Mocktails { get; set; }
    public DbSet<Domain.ingredient> Ingredients { get; set; }
    public DbSet<Domain.mocktail_ingredient> MocktailIngredients { get; set; }
    
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
            builder.HasKey(x => x.id);
            builder.Property(x => x.id).HasColumnName("id");
            builder.Property(x => x.nom).HasColumnName("nom").IsRequired();
            builder.Property(x => x.description).HasColumnName("description");
            builder.Property(x => x.prix).HasColumnName("prix").HasColumnType("decimal(10,2)");
            builder.Property(x => x.image).HasColumnName("image");
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

            builder.Property(x => x.last_modified_at)
                .HasColumnName("last_modified_at")
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .ValueGeneratedOnAdd();
        });


        modelBuilder.Entity<Domain.mocktail_ingredient>(builder =>
        {
            builder.ToTable("mocktail_ingredient");
            builder.HasKey(x => x.id);
            builder.Property(x => x.id).HasColumnName("id");
            builder.Property(x => x.mocktail_id).HasColumnName("mocktail_id");
            builder.Property(x => x.ingredient_id).HasColumnName("ingredient_id");
            builder.Property(x => x.quantite).HasColumnName("quantite").HasColumnType("decimal(10,2)");
            builder.Property(x => x.unite).HasColumnName("unite").HasMaxLength(10);

            // Relations
            builder.HasOne(x => x.Mocktail)
                .WithMany(x => x.MocktailIngredients)
                .HasForeignKey(x => x.mocktail_id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Ingredient)
                .WithMany(x => x.MocktailIngredients)
                .HasForeignKey(x => x.ingredient_id)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}