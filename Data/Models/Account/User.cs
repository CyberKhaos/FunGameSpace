using System.ComponentModel.DataAnnotations;

namespace Models.Account;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string EmailHashed { get; set; } = string.Empty;
    public string PasswordHashed { get; set; } = string.Empty;
}