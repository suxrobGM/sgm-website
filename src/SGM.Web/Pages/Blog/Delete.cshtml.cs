using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SGM.Web.Utils;
using SGM.Domain.Entities.BlogEntities;
using SGM.Domain.Interfaces.Repositories;

namespace SGM.Web.Pages.Blog
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly IBlogRepository _blogRepository;

        public DeleteModel(ImageHelper imageHelper, IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
            _imageHelper = imageHelper;
        }

        [BindProperty]
        public Domain.Entities.BlogEntities.Blog Blog { get; set; }

        public string Tags { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Blog = await _blogRepository.GetByIdAsync(id);
            Tags = Tag.ConvertTagsToString(Blog.Tags);

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

            Blog = await _blogRepository.GetByIdAsync(id);

            if (Blog != null)
            {
                await _blogRepository.DeleteBlogAsync(Blog);
                _imageHelper.RemoveImage(Blog.CoverPhotoPath);
            }

            return RedirectToPage("/Blog/List");
        }
    }
}
