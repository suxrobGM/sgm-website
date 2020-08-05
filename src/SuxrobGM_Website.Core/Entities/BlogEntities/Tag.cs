using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SuxrobGM_Website.Core.Entities.Base;

namespace SuxrobGM_Website.Core.Entities.BlogEntities
{
    public class Tag : EntityBase
    {
        public Tag()
        {

        }

        public Tag(string tagName)
        {
            Name = tagName.Trim();
        }

        [Required]
        [StringLength(40)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public virtual ICollection<BlogTag> BlogTags { get; set; }

        public override string ToString() => Name;
        public static implicit operator Tag(string tagName) => new Tag(tagName);
        public static implicit operator string(Tag tag) => tag.Name;

        public static Tag[] ParseTags(string tagsString, char separator = ',')
        {
            var tags = tagsString.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            var tagsArray = tags.Select(tag => (Tag) tag).ToArray();
            return tagsArray;
        }

        public static string JoinTags(IEnumerable<Tag> tags, char separator = ',')
        {
            return string.Join(separator, tags);
        }
    }
}
