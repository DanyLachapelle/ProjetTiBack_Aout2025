using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;


public class User_account
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
} 