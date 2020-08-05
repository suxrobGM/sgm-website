using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SuxrobGM_Website.Infrastructure.Data;
using SuxrobGM_Website.Web.Utils;

namespace SuxrobGM_Website.Web.Pages.Blog
{
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CreateModel(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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
        public Core.Entities.BlogEntities.Blog Article { get; set; }      

        [BindProperty]
        public IFormFile UploadCoverPhoto { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var currentUser = await _context.Users.FirstAsync(i => i.UserName == User.Identity.Name);
            Article.Slug = Core.Entities.BlogEntities.Blog.CreateSlug(Article.Title);
            Article.Author = currentUser;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (UploadCoverPhoto != null)
            {
                var image = UploadCoverPhoto;
                var fileName = $"{Article.Id}_cover.jpg";
                var fileNameAbsPath = Path.Combine(_env.WebRootPath, "db_files", "img", fileName);
                ImageHelper.ResizeToRectangle(image.OpenReadStream(), fileNameAbsPath);
                Article.CoverPhotoPath = $"/db_files/img/{fileName}";
            }

            if (_context.Articles.Any(i => i.Slug == Article.Slug))
            {
                ModelState.AddModelError("Blog.Slug", "This blog slug exists please change title");
                return Page();
            }
           
            await _context.Articles.AddAsync(Article);
            await _context.SaveChangesAsync();
            return RedirectToPage("/Blog/Index", new { slug = Article.Slug });
        }
    }
}