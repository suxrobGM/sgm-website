using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SuxrobGM.Sdk.Pagination;
using SuxrobGM_Website.Data;
using SuxrobGM_Website.Models;

namespace SuxrobGM_Website.Pages.Blog
{
    public class BlogListModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BlogListModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public PaginatedList<Article> Articles { get; set; }
        public Article[] PopularArticles { get; set; }
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

        public async Task<string[]> GetPopularTagsAsync(IQueryable<Article> articles)
        {
            return await Task.Run(() =>
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