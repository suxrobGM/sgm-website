using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SGM.Domain.Entities.Base;

namespace SGM.Domain.Entities.BlogEntities
{
    public class Tag : EntityBase, IEqualityComparer<Tag>
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

        public virtual IList<Blog> Blogs { get; set; }

        public override string ToString() => Name;
        public static implicit operator Tag(string tagName) => new Tag(tagName);
        public static implicit operator string(Tag tag) => tag.Name;

        public static Tag[] ParseTags(string tagsString, char separator = ',')
        {
            var tags = tagsString.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            var tagsArray = tags.Select(tag => (Tag) tag).ToArray();
            return tagsArray;
        }

        public static string ConvertTagsToString(IEnumerable<Tag> tags, char separator = ',')
        {
            return string.Join(separator, tags);
        }

        public bool Equals(Tag x, Tag y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Name.ToLower() == y.Name.ToLower();
        }

        public int GetHashCode(Tag obj)
        {
            return (obj.Name != null ? obj.Name.GetHashCode() : 0);
        }
    }
}
