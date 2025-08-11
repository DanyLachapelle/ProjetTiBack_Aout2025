using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Api.Services;
using Application.DTOs;
using Application.MappingProfile;
using Application.User.commands;
using Application.User.commands.login;
using Application.Utils;
using Application.Services;
using Application.User_account.commands.resetPassword;
using Application.User.commands.changePassword;
using Application.User.commands.forgotPassword;
using Application.User.commands.resetPassword;
using Infrastructure.User;
using Infrastructure.Mocktail;
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
builder.Services.AddScoped<ICommandHandler<UserAccountChangePasswordCommand,UserAccountChangePasswordOutput>, UserAccountChangePasswordHandler>();
builder.Services.AddScoped<ICommandHandler<UserAccountForgotPasswordCommand, UserAccountForgotPasswordOutput>, UserAccountForgotPasswordHandler>();
builder.Services.AddScoped<ICommandHandler<UserAccountResetPasswordCommand, UserAccountResetPasswordOutput>, UserAccountResetPasswordHandler>();

builder.Services.AddDbContext<DbContext>(dbContextBuilder =>
{
    dbContextBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Email service (Gmail SMTP)
builder.Services.AddScoped<IEmailService, EmailService>();

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