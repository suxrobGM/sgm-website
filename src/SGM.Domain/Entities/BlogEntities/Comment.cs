using System.ComponentModel.DataAnnotations;
using SGM.Domain.Entities.UserEntities;

namespace SGM.Domain.Entities.BlogEntities
{
    public class Comment : EntityBase
    {
        [Required(ErrorMessage = "Please enter content")]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [StringLength(64, ErrorMessage = "Characters must be less than 64")]
        [Display(Name = "Author Name")]
        public string AuthorName { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "This is not valid email address")]
        [StringLength(64, ErrorMessage = "Characters must be less than 64")]
        [Display(Name = "Author Email")]
        public string AuthorEmail { get; set; }

        [Display(Name = "Author")]
        public virtual ApplicationUser Author { get; set; }
        public virtual Comment ParentComment { get; set; }

        [Display(Name = "Blog")]
        public virtual Blog Blog { get; set; }

        public virtual IList<Comment> Replies { get; set; }
    }
}