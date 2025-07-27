using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace Infrastructure.User;

public class DbContext:Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Domain.User_account> Users { get; set; }
    public DbSet<Domain.Mocktail> Mocktails { get; set; }
    public DbSet<Domain.Ingredient> Ingredients { get; set; }
    public DbSet<Domain.mocktail_ingredient> MocktailIngredients { get; set; }
    
    public DbSet<Domain.Sale> Sales { get; set; }
    public DbSet<Domain.SaleItem> SaleItems { get; set; }
    
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
        modelBuilder.Entity<Domain.User_account>(builder =>
        {
            builder.ToTable("User_account"); 
            builder.HasKey(x => x.id); 
            builder.Property(x => x.id).HasColumnName("id");
            builder.Property(x => x.username).HasColumnName("username").IsRequired();
            builder.Property(x => x.email).HasColumnName("email").IsRequired();
            builder.Property(x => x.password).HasColumnName("password").IsRequired();
            builder.Property(x => x.role).HasColumnName("role");
        });

        modelBuilder.Entity<Domain.Mocktail>(builder =>
        {
            builder.ToTable("Mocktail");
            builder.HasKey(x => x.id);
            builder.Property(x => x.id).HasColumnName("id");
            builder.Property(x => x.name).HasColumnName("name").IsRequired();
            builder.Property(x => x.description).HasColumnName("description");
            builder.Property(x => x.price).HasColumnName("price").HasColumnType("decimal(10,2)");
            builder.Property(x => x.image).HasColumnName("image");
        });

        modelBuilder.Entity<Domain.Ingredient>(builder =>
{
    
    builder.ToTable("Ingredient");
    
    // Configuration de la clé primaire
    builder.HasKey(x => x.id);
    builder.Property(x => x.id)
        .HasColumnName("id")
        .ValueGeneratedOnAdd();

    // Configuration des colonnes de base
    builder.Property(x => x.name)
        .HasColumnName("name")
        .HasColumnType("VARCHAR(100)")
        .IsRequired()
        .HasMaxLength(100);
        
    builder.Property(x => x.quantity)
        .HasColumnName("quantity")
        .HasColumnType("DECIMAL(10,2)")
        .IsRequired();
        
    builder.Property(x => x.restock_threshold)
        .HasColumnName("restock_threshold")
        .HasColumnType("DECIMAL(10,2)")
        .IsRequired();
        
    builder.Property(x => x.unit)
        .HasColumnName("unit")
        .HasColumnType("VARCHAR(10)")
        .IsRequired()
        .HasMaxLength(10);

    // Configuration de la colonne allergen avec check constraint
    builder.Property(x => x.allergen)
        .HasColumnName("allergen")
        .HasColumnType("VARCHAR(20)")
        .HasDefaultValue("none")
        .IsRequired()
        .HasMaxLength(20)
        .HasConversion(
            v => v.ToLower(), // Supprime le check null car IsRequired() garantit déjà non-null
            v => v);

    // Configuration du timestamp de modification
    builder.Property(x => x.last_modified_at)
        .HasColumnName("last_modified_at")
        .HasColumnType("DATETIME2")
        .HasDefaultValueSql("CURRENT_TIMESTAMP")
        .ValueGeneratedOnAddOrUpdate();

    // Configuration de l'index unique sur le nom
    builder.HasIndex(x => x.name)
        .IsUnique();

    // Configuration des contraintes CHECK via Fluent API
    builder.ToTable(t => t.HasCheckConstraint("CK_Ingredient_Unit", "unit IN ('g', 'l', 'cl')"));
    
    builder.ToTable(t => t.HasCheckConstraint("CK_Ingredient_Allergen", 
        "allergen IN ('none', 'gluten', 'crustaceans', 'eggs', 'fish', 'peanuts', " +
        "'soybeans', 'milk', 'nuts', 'celery', 'mustard', 'sesame', " +
        "'sulphites', 'lupin', 'molluscs')"));
});


        modelBuilder.Entity<Domain.mocktail_ingredient>(builder =>
        {
            builder.ToTable("mocktail_ingredient");
            builder.HasKey(x => x.id);
            builder.Property(x => x.id).HasColumnName("id");
            builder.Property(x => x.mocktail_id).HasColumnName("mocktail_id");
            builder.Property(x => x.ingredient_id).HasColumnName("ingredient_id");
            builder.Property(x => x.quantity).HasColumnName("quantity").HasColumnType("decimal(10,2)");
            builder.Property(x => x.unit).HasColumnName("unit").HasMaxLength(10);

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
        
        modelBuilder.Entity<Domain.Sale>(builder =>
        {
            builder.ToTable("SALE");
            builder.HasKey(x => x.Id);
    
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
        
            builder.Property(x => x.TotalAmount)
                .HasColumnName("total_amount")
                .HasColumnType("DECIMAL(10,2)")
                .IsRequired();
        
            builder.Property(x => x.SaleDate)
                .HasColumnName("sale_date")
                .HasColumnType("TIMESTAMP")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
            builder.Property(x => x.TableNumber)
                .HasColumnName("table_number")
                .HasColumnType("VARCHAR(10)")
                .HasMaxLength(10);
        
            // Relation avec SaleItems
            builder.HasMany(x => x.SaleItems)
                .WithOne(x => x.Sale)
                .HasForeignKey(x => x.SaleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Domain.SaleItem>(builder =>
        {
            builder.ToTable("SALE_ITEM");
            builder.HasKey(x => x.Id);
    
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
        
            builder.Property(x => x.SaleId)
                .HasColumnName("sale_id")
                .IsRequired();
        
            builder.Property(x => x.MocktailId)
                .HasColumnName("mocktail_id");
        
            builder.Property(x => x.Quantity)
                .HasColumnName("quantity")
                .IsRequired();
        
            builder.Property(x => x.ItemTotal)
                .HasColumnName("item_total")
                .HasColumnType("DECIMAL(10,2)")
                .IsRequired();
        
            // Contrainte CHECK pour quantity
            builder.ToTable(t => t.HasCheckConstraint("CK_SaleItem_Quantity", "quantity > 0"));
    
            // Relations
            builder.HasOne(x => x.Sale)
                .WithMany(x => x.SaleItems)
                .HasForeignKey(x => x.SaleId)
                .OnDelete(DeleteBehavior.Cascade);
        
            builder.HasOne(x => x.Mocktail)
                .WithMany()
                .HasForeignKey(x => x.MocktailId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}