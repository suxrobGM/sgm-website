using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using SuxrobGM.Sdk.Utils;
using SuxrobGM_Website.Core.Interfaces.Entities;

namespace SuxrobGM_Website.Core.Entities.UserEntities
{
    public class ApplicationUser : IdentityUser<string>, IEntity<string>
    {
        [StringLength(32)]
        [Display(Name = "ID")]
        public override string Id { get; set; } = GeneratorId.GenerateComplex();

        [StringLength(40, ErrorMessage = "Characters must be less than 40")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(40, ErrorMessage = "Characters must be less than 40")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(64)]
        [Display(Name = "Profile Photo Path")]
        public string ProfilePhotoPath { get; set; }
    }
}
