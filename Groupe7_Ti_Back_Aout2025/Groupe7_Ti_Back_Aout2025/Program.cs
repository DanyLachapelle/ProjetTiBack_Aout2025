using System.Text;
using Api.Services;
using Application.DTOs;
using Application.Ingredient.commands;
using Application.Ingredient.commands.createIngredient;
using Application.Ingredient.commands.deleteIngredient;
using Application.Ingredient.commands.UpdateLimitIngredient;
using Application.Ingredient.commands.UpdateQuantityIngredient;
using Application.Ingredient.query;
using Application.Ingredient.query.getAllIngredient;
using Application.MappingProfile;
using Application.Mocktails.commands;
using Application.Mocktails.commands.deleteMocktail;
using Application.Mocktails.query;
using Application.Mocktails.query.getbyidMocktail;
using Application.Mocktails.Query.GetByIdMocktail;
using Application.User.commands;
using Application.User.commands.login;
using Application.Utils;
using Application.Services;
using Infrastructure.Ingredient;
using Infrastructure.User;
using Infrastructure.Mocktail;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DbContext = Infrastructure.User.DbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4300",
                "http://localhost:4200",
                "https://localhost:4300",
                "https://localhost:4200"
            ) // Autoriser l'origine Angular
            .AllowAnyHeader()                   // Autoriser tous les en-têtes
            .AllowAnyMethod()                   // Autoriser toutes les méthodes HTTP (GET, POST, etc.)
            .AllowCredentials();                // Autoriser les cookies et credentials
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

builder.Services.AddScoped<TokenService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["cookie"];
            return Task.CompletedTask;
        }
    };
});
// USER
builder.Services.AddScoped<DbContext>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserAccountCommandProcessor>();
builder.Services.AddScoped<ICommandHandler<UserAccountLoginQuery, UserAccountLoginOutput>, UserAccountLoginHandler>();

// ingredient
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IngredientQueryProcessor>();
builder.Services.AddScoped<IngredientCommandProcessor>();
builder.Services.AddScoped<IQueryHandler<IngredientGetAllQuery, IngredientGetAllOutput>, IngredientGetAllHandler>();
builder.Services.AddScoped<ICommandHandler<CreateIngredientQuery, CreateIngredientOutput>, CreateIngredientHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteIngredientQuery, DeleteIngredientOutput>, DeleteIngredientHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateLimitIngredientCommand, UpdateLimitIngredientOutput>, UpdateLimitIngredientHandler>();    
builder.Services.AddScoped<ICommandHandler<UpdateQuantityIngredientQuery, UpdateQuantityIngredientOutput>, UpdateQuantityIngredientHandler>();

// MOCKTAIL
builder.Services.AddScoped<IMocktailRepository, MocktailRepository>();
builder.Services.AddScoped<IMocktailService, MocktailService>();
builder.Services.AddScoped<MocktailQueryProcessor>();
builder.Services.AddScoped<MocktailCommandProcessor>();
builder.Services.AddScoped<IQueryHandler<GetbyidMocktailQuery, MocktailDto>, GetbyidMocktailHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteMocktailCommand, DeleteMocktailOutput>, DeleteMocktailHandler>();
builder.Services.AddDbContext<DbContext>(dbContextBuilder =>
{
    dbContextBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS doit être appelé avant les autres middlewares
app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();