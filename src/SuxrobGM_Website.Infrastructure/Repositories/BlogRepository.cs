using System.Linq;
using System.Threading.Tasks;
using SuxrobGM_Website.Core.Entities.BlogEntities;
using SuxrobGM_Website.Core.Interfaces.Entities;
using SuxrobGM_Website.Core.Interfaces.Repositories;
using SuxrobGM_Website.Infrastructure.Data;

namespace SuxrobGM_Website.Infrastructure.Repositories
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Comment> _commentRepository;
        private readonly IRepository<Tag> _tagRepository;

        public BlogRepository(ApplicationDbContext context,
            IRepository<Comment> commentRepository, 
            IRepository<Tag> tagRepository) : base(context)
        {
            _context = context;
            _commentRepository = commentRepository;
            _tagRepository = tagRepository;
        }

        public Task<Comment> GetCommentByIdAsync(string commentId)
        {
            return _commentRepository.GetByIdAsync(commentId);
        }

        public Task AddBlogAsync(Blog blog)
        {
            blog.Slug = GetVerifiedBlogSlug(blog);
            return AddAsync(blog);
        }

        public Task AddCommentAsync(Blog blog,Comment comment)
        {
            blog.Comments.Add(comment);
            return UpdateAsync(blog);
        }

        public Task AddReplyToCommentAsync(Comment parentComment, Comment childComment)
        {
            parentComment.Replies.Add(childComment);
            return _commentRepository.UpdateAsync(parentComment);
        }

        public Task UpdateBlogAsync(Blog blog)
        {
            blog.Slug = GetVerifiedBlogSlug(blog);
            return UpdateAsync(blog);
        }

        public async Task UpdateTagsAsync(Blog blog, params Tag[] tags)
        {
            foreach (var tag in tags)
            {
                // ReSharper disable once SpecifyStringComparison
                var originTag = await _tagRepository.GetAsync(i => i.Name.ToLower() == tag.Name.ToLower());

                if (originTag == null)
                {
                    originTag = new Tag(tag);
                    await _tagRepository.AddAsync(originTag);
                }

                // ReSharper disable once SpecifyStringComparison
                if (blog.Tags.Any(i => i.Name.ToLower() == originTag.Name.ToLower()))
                {
                    continue;
                }

                blog.Tags.Add(originTag);
            }

            await UpdateAsync(blog);
        }

        public async Task DeleteBlogAsync(Blog blog)
        {
            if (blog == null)
            {
                return;
            }

            foreach (var comment in blog.Comments)
            {
                await RemoveChildCommentsAsync(comment); // remove child comments
                _context.Remove(comment); // remove root comment
            }

            RemoveEmptyTags(); // remove empty tags
            await DeleteAsync(blog);
        }

        public async Task DeleteCommentAsync(Comment comment)
        {
            if (comment == null)
            {
                return;
            }

            await RemoveChildCommentsAsync(comment);
            await _commentRepository.DeleteByIdAsync(comment.Id);
        }

        private async Task RemoveChildCommentsAsync(Comment comment)
        {
            foreach (var reply in comment.Replies)
            {
                await RemoveChildCommentsAsync(reply);
                _context.Remove(reply);
            }
        }

        private void RemoveEmptyTags()
        {
            var emptyTags = _tagRepository.GetQuery(i => i.Blogs.Count == 0);
            _context.RemoveRange(emptyTags);
        }

        private string GetVerifiedBlogSlug(ISlugifiedEntity slugifiedEntity)
        {
            var slug = slugifiedEntity.Slug;
            var verifiedSlug = slug;
            var alreadyExistsSlug = _context.Set<Blog>().Any(i => i.Slug == verifiedSlug && i.Id != slugifiedEntity.Id);

            var count = 0;
            while (alreadyExistsSlug)
            {
                verifiedSlug = slug.Insert(0, $"{++count}-");
                alreadyExistsSlug = _context.Set<Blog>().Any(i => i.Slug == verifiedSlug && i.Id != slugifiedEntity.Id);
            }

            return verifiedSlug;
        }
    }
}
