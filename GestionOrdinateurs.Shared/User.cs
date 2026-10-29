using System.ComponentModel.DataAnnotations;

// entité user avec son role
public class User
{
    public enum RoleEnum
    {
        Admin,
        Gestionnaire
    }

    public int Id { get; set; }

    [Required]
    public string Login { get; set; } = string.Empty;

    public RoleEnum Role { get; set; }
}
