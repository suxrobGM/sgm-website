using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using SuxrobGM.Sdk.Extensions;
using SuxrobGM_Website.Core.Entities.Base;

namespace SuxrobGM_Website.Core.Entities.BlogEntities
{
    public class Blog : ArticleBase
    {
        [StringLength(64)]
        [Display(Name = "Cover Photo Path")]
        public string CoverPhotoPath { get; set; }

        [Required(ErrorMessage = "Please enter the summary of article")]
        [StringLength(200, ErrorMessage = "Characters must be less than 200")]
        [Display(Name = "Summary")]
        public string Summary { get; set; }

        [Display(Name = "View Count")]
        public int ViewCount { get; set; }

        public virtual ICollection<BlogTag> BlogTags { get; set; } = new List<BlogTag>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

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

        public static string GetShortContent(string articleContent, int length)
        {
            var content = HttpUtility.HtmlDecode(articleContent);
            content = Regex.Replace(content, @"<(.|\n)*?>", "");            
            content = content.Substring(0, length).Trim() + "...";
            return content;
        }
    }
}
