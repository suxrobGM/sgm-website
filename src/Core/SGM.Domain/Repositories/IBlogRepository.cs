using SGM.Domain.Entities.BlogEntities;

namespace SGM.Domain.Repositories;

public interface IBlogRepository : IRepository<Blog>
{
    Task<Comment> GetCommentByIdAsync(string commentId);
    Task AddBlogAsync(Blog blog);
    Task AddCommentAsync(Blog blog, Comment comment);
    Task AddReplyToCommentAsync(Comment parentComment, Comment childComment);
    Task UpdateBlogAsync(Blog blog, bool verifySlug = true);
    Task UpdateTagsAsync(Blog blog, params Tag[] tags);
    Task DeleteBlogAsync(Blog blog);
    Task DeleteCommentAsync(Comment comment);
}
