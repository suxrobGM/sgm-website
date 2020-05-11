using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using SuxrobGM.Sdk.Extensions;
using SuxrobGM.Sdk.Utils;

namespace SuxrobGM_Website.Models
{
    public class Article
    {
        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        [Required]
        [StringLength(64, ErrorMessage = "Characters must be less than 64")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(200, ErrorMessage = "Characters must be less than 200")]
        public string Summary { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "Article url")]
        [StringLength(64)]
        public string Slug { get; set; }

        [StringLength(64)]
        public string CoverPhotoUrl { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public int ViewCount { get; set; }

        [Display(Name = "Author")]
        [StringLength(32)]
        public string AuthorId { get; set; }
        public string Tags { get; set; }

        public virtual User Author { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public string[] GetTags()
        {
            return Tags.Split(',');
        }

        public static string CreateSlug(string title, bool useHypen = true, bool useLowerLetters = true)
        {
            var url = title.TranslateToLatin();
            
            // invalid chars           
            url = Regex.Replace(url, @"[^A-Za-z0-9\s-]", "");

            // convert multiple spaces into one space 
            url = Regex.Replace(url, @"\s+", " ").Trim();
            var words = url.Split().Where(str => !string.IsNullOrWhiteSpace(str));
            url = string.Join(useHypen ? '-' : '_', words);

            if (useLowerLetters)
                url = url.ToLower();

            return url;
        }
    }
}
