using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
using Application.Mocktails.commands.createMocktail;
using Application.Mocktails.commands.deleteMocktail;
using Application.Mocktails.commands.updateMocktail;
using Application.Mocktails.query;
using Application.Mocktails.query.getAllMocktail;
using Application.Mocktails.query.getbyidMocktail;
using Application.Mocktails.Query.GetByIdMocktail;
using Application.Sales.commands;
using Application.Sales.commands.CreateSale;
using Application.Sales.commands.DeleteSale;
using Application.Sales.commands.UpdateSale;
using Application.Sales.query;
using Application.Sales.query.GetSalesByDate;
using Application.Sales.query.GetSalesById;
using Application.User.commands;
using Application.User.commands.login;
using Application.Utils;
using Application.Services;
using Infrastructure.Ingredient;
using Infrastructure.User;
using Infrastructure.Mocktail;
using Infrastructure.Sale;
using Infrastructure.User.Sale;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
builder.Services.AddScoped<ICommandHandler<UserAccountLoginCommand, UserAccountLoginOutput>, UserAccountLoginHandler>();

// ingredient
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IngredientQueryProcessor>();
builder.Services.AddScoped<IngredientCommandProcessor>();
builder.Services.AddScoped<IQueryHandler<IngredientGetAllQuery, IngredientGetAllOutput>, IngredientGetAllHandler>();
builder.Services.AddScoped<ICommandHandler<CreateIngredientCommand, CreateIngredientOutput>, CreateIngredientHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteIngredientCommand, DeleteIngredientOutput>, DeleteIngredientHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateLimitIngredientCommand, UpdateLimitIngredientOutput>, UpdateLimitIngredientHandler>();    
builder.Services.AddScoped<ICommandHandler<UpdateQuantityIngredientCommand, UpdateQuantityIngredientOutput>, UpdateQuantityIngredientHandler>();

// MOCKTAIL
builder.Services.AddScoped<IMocktailRepository, MocktailRepository>();
//builder.Services.AddScoped<IMocktailService, MocktailService>();
builder.Services.AddScoped<MocktailQueryProcessor>();
builder.Services.AddScoped<MocktailCommandProcessor>();
builder.Services.AddScoped<IQueryHandler<GetbyidMocktailQuery, MocktailDto>, GetbyidMocktailHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteMocktailCommand, DeleteMocktailOutput>, DeleteMocktailHandler>();
builder.Services.AddScoped<ICommandHandler<CreateMocktailCommand, CreateMocktailOutput>, CreateMocktailHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateMocktailCommand, UpdateMocktailOutput>, UpdateMocktailHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllMocktailQuery, List<MocktailDto>>, GetAllMocktailHandler>();


builder.Services.AddDbContext<DbContext>(dbContextBuilder =>
{
    dbContextBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Sale
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<SaleCommandProcessor>();
builder.Services.AddScoped<SalesQueryProcessor>();
builder.Services.AddScoped<IQueryHandler<GetSalesByIdQuery, GetSalesByIdOutput>, GetSalesByIdHandler>();
builder.Services.AddScoped<IQueryHandler<GetSalesByDateQuery, GetSalesByDateOutput>, GetSalesByDateHandler>();
builder.Services.AddScoped<ICommandHandler<CreateSaleCommand, CreateSaleOutput>, CreateSaleHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateSaleCommand, UpdateSaleOutput>, UpdateSaleHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteSaleCommand, DeleteSaleOutput>, DeleteSaleHandler>();

var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = "swagger";
});
// CORS doit être appelé avant les autres middlewares
app.UseCors("AllowAngularApp");

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();


app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});
app.Run();
app.UseCors(policy =>
    policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());