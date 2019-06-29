using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SuxrobGM.Sdk.Utils;

namespace SuxrobGM_Resume.Models
{
    public class Article
    {
        public Article()
        {
            Id = GeneratorId.GenerateLong();
            CreatedTime = DateTime.Now;
            Comments = new List<Comment>();
        }

        public string Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(2000, ErrorMessage = "Characters must be less than 2000")]
        public string Summary { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Article url")]
        public string Url { get; set; }

        [Required]
        public string Tags { get; set; }

        public string CoverPhotoUrl { get; set; }
        public DateTime CreatedTime { get; set; }

        [Display(Name = "Author")]
        public string AuthorId { get; set; }
        public virtual User Author { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public string GetRelativeUrl()
        {
            return Url.Remove(0, "/Blog/".Length);
        }     
    }
}
