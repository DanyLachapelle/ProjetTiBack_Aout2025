using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;


public class UserAccount
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("username")]
    public string Username { get; set; } = string.Empty;

    [Column("email")]
    public string Email { get; set; } = string.Empty;
    
    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [Column("role")]
    public string Role { get; set; } = string.Empty;
    
    public void SetHashedPassword(string plainPassword)
    {
        if (string.IsNullOrWhiteSpace(plainPassword))
            throw new ValidationException("Password cannot be empty");

        // Utilisez une librairie comme BCrypt.Net
        this.Password = BCrypt.Net.BCrypt.HashPassword(plainPassword);
    }

    public bool VerifyPassword(string plainPassword)
    {
        if (string.IsNullOrEmpty(this.Password))
            return false;

        return BCrypt.Net.BCrypt.Verify(plainPassword, this.Password);
    }
} 

