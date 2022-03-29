using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SGM.Domain.Entities.UserEntities;

public class ApplicationUser : IdentityUser<string>, IAggregateRoot
{
    public ApplicationUser()
    {
        ProfilePhotoPath = "/img/default_user_avatar.png";
        Timestamp = DateTime.Now;
    }

    [StringLength(40, ErrorMessage = "Characters must be less than 40")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [StringLength(40, ErrorMessage = "Characters must be less than 40")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [StringLength(64)]
    [Display(Name = "Profile Photo Path")]
    public string ProfilePhotoPath { get; set; }

    [Display(Name = "Registration Date")]
    public DateTime Timestamp { get; set; }
}
