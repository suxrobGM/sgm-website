using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageMagick;
using Microsoft.AspNetCore.Identity;

namespace SuxrobGM_Resume.Models
{
    public class User : IdentityUser<string>
    {
        public User()
        {
            Blogs = new List<Blog>();
            Comments = new List<Comment>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePhotoUrl { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }        
    }
}
