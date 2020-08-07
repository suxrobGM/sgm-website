using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM_Website.Core.Interfaces.Repositories;
using SuxrobGM_Website.Infrastructure.Data;
using SuxrobGM_Website.Web.Utils;

namespace SuxrobGM_Website.Web.Pages.Blog
{
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public class EditModel : PageModel
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IWebHostEnvironment _env;

        public EditModel(IBlogRepository blogRepository, IWebHostEnvironment env)
        {
            _blogRepository = blogRepository;
            _env = env;
        }

        [BindProperty]
        public Core.Entities.BlogEntities.Blog Blog { get; set; }

        [BindProperty]
        public IFormFile UploadCoverPhoto { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Blog = await _blogRepository.GetByIdAsync<Core.Entities.BlogEntities.Blog>(id);

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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var blog = await _blogRepository.GetByIdAsync<Core.Entities.BlogEntities.Blog>(Blog.Id);
            blog.Title = Blog.Title;
            blog.Summary = Blog.Summary;
            blog.Content = Blog.Content;
            blog.Tags = Blog.Tags;
            blog.Slug = Blog.CreateSlug(Blog.Title);

            if (UploadCoverPhoto != null)
            {
                var image = UploadCoverPhoto;
                var fileName = $"{blog.Id}_cover.jpg";
                var fileNameAbsPath = Path.Combine(_env.WebRootPath, "db_files", "img", fileName);
                ImageHelper.ResizeToRectangle(image.OpenReadStream(), fileNameAbsPath);
                blog.CoverPhotoPath = $"/db_files/img/{fileName}";
            }

            await _blogRepository.UpdateBlogAsync(blog);
            return RedirectToPage("/Blog/Index", new { slug = blog.Slug });
        }
    }
}