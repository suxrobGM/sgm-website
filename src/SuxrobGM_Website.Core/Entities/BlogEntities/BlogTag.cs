using System.ComponentModel.DataAnnotations;

namespace SuxrobGM_Website.Core.Entities.BlogEntities
{
    public class BlogTag
    {
        public string BlogId { get; set; }
        public virtual Blog Blog { get; set; }

        public string TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
