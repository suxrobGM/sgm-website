using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SuxrobGM.Sdk.Utils;

namespace SuxrobGM_Website.Models
{
    public class Comment
    {
        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();
        public string Text { get; set; }

        [StringLength(64, ErrorMessage = "Characters must be less than 64")]
        public string AuthorName { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [DataType(DataType.EmailAddress, ErrorMessage = "This is not valid email address")]
        [StringLength(64, ErrorMessage = "Characters must be less than 64")]
        public string AuthorEmail { get; set; }

        [StringLength(32)]
        public string AuthorId { get; set; }
        public virtual User Author { get; set; }

        [StringLength(32)]
        public string ParentId { get; set; }
        public virtual Comment Parent { get; set; }

        [StringLength(32)]
        public string ArticleId { get; set; }
        public virtual Article Article { get; set; }

        public virtual ICollection<Comment> Replies { get; set; }
    }
}