using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM.Sdk.Extensions;
using SuxrobGM_Website.Core.Entities.BlogEntities;
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

        public class InputModel
        {
            public Core.Entities.BlogEntities.Blog Blog { get; set; }
            public IFormFile UploadCoverPhoto { get; set; }
            public string Tags { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var tags = Tag.ParseTags(Input.Tags);
            Input.Blog.Slug = Input.Blog.Title.Slugify();
            Input.Blog.Author = currentUser;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Input.UploadCoverPhoto != null)
            {
                var image = Input.UploadCoverPhoto;
                var fileName = $"{Input.Blog.Id}_cover.jpg";
                var fileNameAbsPath = Path.Combine(_env.WebRootPath, "db_files", "img", fileName);
                ImageHelper.ResizeToRectangle(image.OpenReadStream(), fileNameAbsPath);
                Input.Blog.CoverPhotoPath = $"/db_files/img/{fileName}";
            }

            await _blogRepository.UpdateTagsAsync(Input.Blog, false, tags);
            await _blogRepository.AddBlogAsync(Input.Blog);
            return RedirectToPage("/Blog/Index", new { slug = Input.Blog.Slug });
        }
    }
}