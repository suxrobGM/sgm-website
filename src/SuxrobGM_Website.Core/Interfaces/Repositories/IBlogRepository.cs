using System.Threading.Tasks;
using SuxrobGM_Website.Core.Entities.BlogEntities;

namespace SuxrobGM_Website.Core.Interfaces.Repositories
{
    public interface IBlogRepository : IRepository<Blog>
    {
        Task<Comment> GetCommentById(string commentId);
        Task AddBlogAsync(Blog blog);
        Task AddCommentAsync(Blog blog, Comment comment);
        Task AddReplyToCommentAsync(Comment parentComment, Comment childComment);
        Task UpdateBlogAsync(Blog blog);
        Task UpdateTagsAsync(Blog blog, bool saveChanges = true, params Tag[] tags);
        Task DeleteBlogAsync(Blog blog);
        Task DeleteCommentAsync(Comment comment, bool saveChanges = true);
    }
}
