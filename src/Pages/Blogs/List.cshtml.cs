using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServerSideAnalytics;
using SuxrobGM.Sdk.Pagination;
using SuxrobGM_Resume.Data;
using SuxrobGM_Resume.Models;

namespace SuxrobGM_Resume.Pages.Blogs
{
    public class BlogsListModel : PageModel
    {
        private ApplicationDbContext _context;
        //private IAnalyticStore _analyticStore;

        public BlogsListModel(ApplicationDbContext context)
        {
            _context = context;
            //_analyticStore = analyticStore;
        }

        public PaginatedList<Blog> Blogs { get; set; }
        public int PageIndex { get; set; }

        public async Task OnGetAsync(int pageIndex = 1)
        {
            PageIndex = pageIndex;
            Blogs = await PaginatedList<Blog>.CreateAsync(_context.Blogs.OrderByDescending(i => i.CreatedTime), pageIndex);
            //var t = await _analyticStore.IpAddressesAsync(DateTime.Now);
        }
    }
}