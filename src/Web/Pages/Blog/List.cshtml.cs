using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public int PageIndex { get; set; }

        public void OnGet(int pageIndex = 1, string tag = null)
        {
            PageIndex = pageIndex;

            if (tag != null)
            {
                var taggedArticles = _context.Articles.Where(i => i.Tags.Contains(tag));
                Articles = PaginatedList<Article>.Create(taggedArticles.OrderByDescending(i => i.Timestamp), pageIndex, 5);
            }
            else
            {
                Articles = PaginatedList<Article>.Create(_context.Articles.OrderByDescending(i => i.Timestamp), pageIndex, 5);
            }
            
            PopularArticles = _context.Articles.OrderByDescending(i => i.ViewCount).Take(5).ToArray();
            PopularTags = GetPopularTags();
        }

        public string GetShortContent(string articleContent)
        {
            articleContent = articleContent.Replace('\'', '\"').Replace("\r\n", " ");
            var re = new Regex("(src|srcset|href)=\".+?\"");
            var matchedSrc = re.Matches(articleContent).ToArray();

            return matchedSrc.Aggregate(articleContent, (current, match) => current.Replace(match.Value, ""));
        }

        public string[] GetPopularTags()
        {
            var articles = _context.Articles;
            var tags = new List<string>();

            foreach (var article in articles)
            {
                tags.AddRange(article.GetTags());
            }

            var popularTags = tags.GroupBy(str => str)
                .Select(i => new { Name = i.Key, Count = i.Count() })
                .OrderByDescending(k => k.Count).Select(i => i.Name).Take(10).ToArray();

            return popularTags;
        }
    }
}