using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM.Sdk.Pagination;
using SuxrobGM_Resume.Data;
using SuxrobGM_Resume.Models;

namespace SuxrobGM_Resume.Pages.Blogs
{    
    public class BlogsListModel : PageModel
    {
        private ApplicationDbContext _context;

        public BlogsListModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public PaginatedList<Blog> Blogs { get; set; }
        public int PageIndex { get; set; }

        public async Task OnGetAsync(int pageIndex = 1)
        {
            PageIndex = pageIndex;
            Blogs = await PaginatedList<Blog>.CreateAsync(_context.Blogs, pageIndex);
        }
    }
}