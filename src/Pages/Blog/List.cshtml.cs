using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM.Sdk.Pagination;
using SuxrobGM_Resume.Data;
using SuxrobGM_Resume.Models;

namespace SuxrobGM_Resume.Pages.Blog
{
    public class BlogListModel : PageModel
    {
        private ApplicationDbContext _context;

        public BlogListModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public PaginatedList<Article> Articles { get; set; }
        public int PageIndex { get; set; }

        public async Task OnGetAsync(int pageIndex = 1)
        {
            PageIndex = pageIndex;
            Articles = await PaginatedList<Article>.CreateAsync(_context.Articles.OrderByDescending(i => i.CreatedTime), pageIndex);
        }
    }
}