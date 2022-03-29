using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SGM.Domain.Entities.UserEntities;

public enum Role
{
    SuperAdmin,
    Admin,
    Moderator,
    Editor
}

public class ApplicationRole : IdentityRole, IAggregateRoot
{
    public ApplicationRole(Role role) : base(role.ToString())
    {
        Role = role;
        Timestamp = DateTime.Now;
    }

    public Role Role { get; set; }

    [StringLength(250, ErrorMessage = "Characters must be less than 250")]
    [Display(Name = "Description")]
    public string Description { get; set; }

    [Display(Name = "Timestamp")]
    public DateTime Timestamp { get; set; }
}
