using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SuxrobGM.Sdk.Utils;

namespace SuxrobGM_Resume.Models
{
    public class Comment
    {
        public Comment()
        {
            Id = GeneratorId.GenerateLong();
            CreatedTime = DateTime.Now;
        }

        public string Id { get; set; }
        public string Text { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreatedTime { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "This is not valid email address")]
        public string AuthorEmail { get; set; }

        public string AuthorId { get; set; }
        public virtual User Author { get; set; }
        
        public string ParentId { get; set; }
        public virtual Comment Parent { get; set; }

        public string BlogId { get; set; }
        public virtual Blog Blog { get; set; }

        public virtual ICollection<Comment> Replies { get; set; }
    }
}