using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

[Table("utilisateur")]
public class User
{
    [Column("id")]
    public int id { get; set; }

    [Column("login")]
    public string pseudo { get; set; } = string.Empty;

    [Column("mot_passe")]
    public string password { get; set; } = string.Empty;

    [Column("role")]
    public string role { get; set; } = string.Empty;
} 