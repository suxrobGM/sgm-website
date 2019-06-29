using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServerSideAnalytics;
using SuxrobGM.Sdk.Pagination;
using SuxrobGM_Resume.Data;
using SuxrobGM_Resume.Models;

namespace SuxrobGM_Resume.Pages.Blog
{
    public class BlogListModel : PageModel
    {
        private ApplicationDbContext _context;
        //private IAnalyticStore _analyticStore;

        public BlogListModel(ApplicationDbContext context)
        {
            _context = context;
            //_analyticStore = analyticStore;
        }

        public PaginatedList<Article> Articles { get; set; }
        public int PageIndex { get; set; }

        public async Task OnGetAsync(int pageIndex = 1)
        {
            PageIndex = pageIndex;
            Articles = await PaginatedList<Article>.CreateAsync(_context.Articles.OrderByDescending(i => i.CreatedTime), pageIndex);
            //var t = await _analyticStore.IpAddressesAsync(DateTime.Now);
        }
    }
}