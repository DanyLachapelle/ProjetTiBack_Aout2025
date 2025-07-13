using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;


public class UserAccount
{
    public int id { get; set; }

    
    public string username { get; set; } = string.Empty;

    
    public string password { get; set; } = string.Empty;

    
    public string role { get; set; } = string.Empty;
} 