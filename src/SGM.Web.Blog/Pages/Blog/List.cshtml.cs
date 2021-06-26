using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM.Sdk.AspNetCore.Pagination;
using SGM.Domain.Interfaces.Repositories;

namespace SGM.Web.Blog.Pages
{
    public class ListBlogModel : PageModel
    {
        private readonly IBlogRepository _blogRepository;

        public ListBlogModel(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public PaginatedList<Domain.Entities.BlogEntities.Blog> Blogs { get; set; }
        public Domain.Entities.BlogEntities.Blog[] PopularArticles { get; set; }
        public string[] PopularTags { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageIndex = 1, string tag = null)
        {
            var blogs = _blogRepository.GetQuery();

            if (tag != null)
            {
                var taggedBlogs = _blogRepository.GetQuery(i => i.Tags.Select(t => t.Name.Trim().ToLower())
                    .Contains(tag.Trim().ToLower()));

                Blogs = PaginatedList<Domain.Entities.BlogEntities.Blog>.Create(taggedBlogs, pageIndex, 5);
            }
            else
            {
                Blogs = PaginatedList<Domain.Entities.BlogEntities.Blog>.Create(blogs.OrderByDescending(i => i.Timestamp), pageIndex, 5);
            }

            PopularArticles = blogs.OrderByDescending(i => i.ViewCount).Take(5).ToArray();
            PopularTags = await GetPopularTagsAsync(blogs);
            return Page();
        }

        public Task<string[]> GetPopularTagsAsync(IQueryable<Domain.Entities.BlogEntities.Blog> blogs)
        {
            return Task.Run(() =>
            {
                var tags = new List<string>();
                var blogsList = blogs.ToList();

                foreach (var blog in blogsList)
                {
                    tags.AddRange(blog.Tags.Select(i => i.Name));
                }

                var popularTags = tags.GroupBy(str => str)
                    .Select(i => new {Name = i.Key, Count = i.Count()})
                    .OrderByDescending(k => k.Count).Select(i => i.Name).Take(10).ToArray();

                return popularTags;
            });
        }
    }
}