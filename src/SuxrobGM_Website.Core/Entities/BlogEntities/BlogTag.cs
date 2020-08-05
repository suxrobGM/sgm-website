using System.ComponentModel.DataAnnotations;

namespace SuxrobGM_Website.Core.Entities.BlogEntities
{
    public class BlogTag
    {
        [StringLength(32)]
        public string BlogId { get; set; }
        public virtual Blog Blog { get; set; }

        [StringLength(32)]
        public string TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
