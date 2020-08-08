using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM_Website.Core.Entities.BlogEntities;
using SuxrobGM_Website.Core.Interfaces.Repositories;

namespace SuxrobGM_Website.Web.Pages.Blog
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IWebHostEnvironment _env;

        public DeleteModel(IBlogRepository blogRepository, IWebHostEnvironment env)
        {
            _blogRepository = blogRepository;
            _env = env;
        }

        [BindProperty]
        public Core.Entities.BlogEntities.Blog Blog { get; set; }

        public string Tags { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Blog = await _blogRepository.GetByIdAsync<Core.Entities.BlogEntities.Blog>(id);
            Tags = Tag.JoinTags(Blog.BlogTags.Select(i => i.Tag));

            if (Blog == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Blog = await _blogRepository.GetByIdAsync<Core.Entities.BlogEntities.Blog>(id);

            await _blogRepository.DeleteBlogAsync(Blog);
            return RedirectToPage("/Blog/List");
        }
    }
}
