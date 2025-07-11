using System.Text;
using Api.Services;
using Application.MappingProfile;
using Application.User.commands;
using Application.User.commands.login;
using Application.Utils;
using Application.Services;
using Infrastructure.User;
using Infrastructure.Mocktail;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
builder.Services.AddScoped<UserContext>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserCommandProcessor>();
builder.Services.AddScoped<IQueryHandler<UserLoginQuery, UserLoginOutput>, UserLoginHandler>();

// MOCKTAIL
builder.Services.AddScoped<IMocktailRepository, MocktailRepository>();
builder.Services.AddScoped<IMocktailService, MocktailService>();

builder.Services.AddDbContext<UserContext>(dbContextBuilder =>
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