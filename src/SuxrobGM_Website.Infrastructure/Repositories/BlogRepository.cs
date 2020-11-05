using System.Linq;
using System.Threading.Tasks;
using SuxrobGM_Website.Core.Entities.BlogEntities;
using SuxrobGM_Website.Core.Interfaces.Entities;
using SuxrobGM_Website.Core.Interfaces.Repositories;
using SuxrobGM_Website.Infrastructure.Data;

namespace SuxrobGM_Website.Infrastructure.Repositories
{
    public class BlogRepository : Repository, IBlogRepository
    {
        private readonly ApplicationDbContext _context;

        public BlogRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
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
            return UpdateAsync(parentComment);
        }

        public Task UpdateBlogAsync(Blog blog)
        {
            blog.Slug = GetVerifiedBlogSlug(blog);
            return UpdateAsync(blog);
        }

        public async Task UpdateTagsAsync(Blog blog, bool saveChanges = true, params Tag[] tags)
        {
            foreach (var tag in tags)
            {
                // ReSharper disable once SpecifyStringComparison
                var originTag = await GetAsync<Tag>(i => i.Name.ToLower() == tag.Name.ToLower());

                if (originTag == null)
                {
                    originTag = new Tag(tag);
                    await _context.Set<Tag>().AddAsync(originTag);
                }

                // ReSharper disable once SpecifyStringComparison
                if (blog.BlogTags.Any(i => i.Tag.Name.ToLower() == originTag.Name.ToLower()))
                {
                    continue;
                }

                blog.BlogTags.Add(new BlogTag
                {
                    Tag = originTag
                });
            }

            if (saveChanges)
            {
                await UpdateAsync(blog);
            }
        }

        public async Task DeleteBlogAsync(Blog blog)
        {
            foreach (var comment in blog.Comments)
            {
                await DeleteCommentAsync(comment, false);
            }

            await DeleteAsync(blog);
        }

        public async Task DeleteCommentAsync(Comment comment, bool saveChanges = true)
        {
            await RemoveChildrenCommentsAsync(comment);
            var rootComment = _context.Set<Comment>().FirstOrDefault(i => i.Id == comment.Id);

            if (rootComment != null)
            {
                _context.Remove(rootComment);
            }

            if (saveChanges)
            {
                await _context.SaveChangesAsync();
            }
        }

        private async Task RemoveChildrenCommentsAsync(Comment comment)
        {
            foreach (var reply in comment.Replies)
            {
                await RemoveChildrenCommentsAsync(reply);
                _context.Remove(reply);
            }
        }

        private string GetVerifiedBlogSlug(ISlugifiedEntity slugifiedEntity)
        {
            var slug = slugifiedEntity.Slug;
            var verifiedSlug = slug;
            var hasSameSlug = _context.Set<Blog>().Any(i => i.Slug == verifiedSlug);

            var count = 0;
            while (hasSameSlug)
            {
                verifiedSlug = slug.Insert(0, $"{++count}-");
                hasSameSlug = _context.Set<Blog>().Any(i => i.Slug == verifiedSlug);
            }

            return verifiedSlug;
        }
    }
}
