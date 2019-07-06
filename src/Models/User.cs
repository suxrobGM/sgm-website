using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SuxrobGM_Website.Models
{
    public class User : IdentityUser<string>
    {
        public User()
        {
            Articles = new List<Article>();
            Comments = new List<Comment>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePhotoUrl { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }        
    }
}
