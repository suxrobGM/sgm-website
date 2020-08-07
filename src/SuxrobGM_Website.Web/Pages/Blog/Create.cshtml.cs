using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM_Website.Core.Entities.UserEntities;
using SuxrobGM_Website.Core.Interfaces.Repositories;
using SuxrobGM_Website.Web.Utils;

namespace SuxrobGM_Website.Web.Pages.Blog
{
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public class CreateModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBlogRepository _blogRepository;
        private readonly IWebHostEnvironment _env;

        public CreateModel(UserManager<ApplicationUser> userManager,
            IBlogRepository blogRepository, IWebHostEnvironment env)
        {
            _blogRepository = blogRepository;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            ViewData.Add("toolbars", new[]
            {
                "Bold", "Italic", "Underline", "StrikeThrough",
                "FontName", "FontSize", "FontColor", "BackgroundColor",
                "LowerCase", "UpperCase", "|",
                "Formats", "Alignments", "OrderedList", "UnorderedList",
                "Outdent", "Indent", "|",
                "CreateTable", "CreateLink", "Image", "|", "ClearFormat", "Print",
                "SourceCode", "FullScreen", "|", "Undo", "Redo"
            });
            return Page();
        }

        [BindProperty]
        public Core.Entities.BlogEntities.Blog Blog { get; set; }      

        [BindProperty]
        public IFormFile UploadCoverPhoto { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            Blog.Slug = Core.Entities.BlogEntities.Blog.CreateSlug(Blog.Title);
            Blog.Author = currentUser;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (UploadCoverPhoto != null)
            {
                var image = UploadCoverPhoto;
                var fileName = $"{Blog.Id}_cover.jpg";
                var fileNameAbsPath = Path.Combine(_env.WebRootPath, "db_files", "img", fileName);
                ImageHelper.ResizeToRectangle(image.OpenReadStream(), fileNameAbsPath);
                Blog.CoverPhotoPath = $"/db_files/img/{fileName}";
            }

            await _blogRepository.AddBlogAsync(Blog);
            return RedirectToPage("/Blog/Index", new { slug = Blog.Slug });
        }
    }
}