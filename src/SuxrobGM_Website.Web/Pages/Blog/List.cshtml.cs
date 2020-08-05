using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM.Sdk.AspNetCore.Pagination;
using SuxrobGM_Website.Infrastructure.Data;

namespace SuxrobGM_Website.Web.Pages.Blog
{
    public class ListModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ListModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public PaginatedList<Core.Entities.BlogEntities.Blog> Articles { get; set; }
        public Core.Entities.BlogEntities.Blog[] PopularArticles { get; set; }
        public string[] PopularTags { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageIndex = 1, string tag = null)
        {
            _context.ChangeTracker.LazyLoadingEnabled = false;
            var articles = _context.Articles.Include(i => i.Comments).AsNoTracking();

            if (tag != null)
            {
                var taggedArticles = articles.Where(i => i.Tags.ToLower().Contains(tag.ToLower()));
                Articles = await PaginatedList<Article>.CreateAsync(taggedArticles.OrderByDescending(i => i.Timestamp), pageIndex, 5);
            }
            else
            {
                Articles = await PaginatedList<Article>.CreateAsync(articles.OrderByDescending(i => i.Timestamp), pageIndex, 5);
            }
            
            PopularArticles = articles.OrderByDescending(i => i.ViewCount).Take(5).ToArray();
            PopularTags = await GetPopularTagsAsync(articles);
            return Page();
        }

        public Task<string[]> GetPopularTagsAsync(IQueryable<Article> articles)
        {
            return Task.Run(() =>
            {
                var tags = new List<string>();

                foreach (var article in articles)
                {
                    tags.AddRange(article.GetTags());
                }

                var popularTags = tags.GroupBy(str => str)
                    .Select(i => new {Name = i.Key, Count = i.Count()})
                    .OrderByDescending(k => k.Count).Select(i => i.Name).Take(10).ToArray();

                return popularTags;
            });
        }
    }
}