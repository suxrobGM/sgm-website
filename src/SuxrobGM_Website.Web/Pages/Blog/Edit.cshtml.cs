using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM.Sdk.Extensions;
using SuxrobGM_Website.Core.Entities.BlogEntities;
using SuxrobGM_Website.Core.Interfaces.Repositories;
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

        public class InputModel
        {
            public Core.Entities.BlogEntities.Blog Blog { get; set; }
            public IFormFile UploadCoverPhoto { get; set; }
            public string Tags { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var blog = await _blogRepository.GetByIdAsync<Core.Entities.BlogEntities.Blog>(id);

            Input = new InputModel()
            {
                Blog = blog,
                Tags = Tag.JoinTags(blog.BlogTags.Select(i => i.Tag))
            };

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

            var blog = await _blogRepository.GetByIdAsync<Core.Entities.BlogEntities.Blog>(Input.Blog.Id);
            blog.Title = Input.Blog.Title;
            blog.Summary = Input.Blog.Summary;
            blog.Content = Input.Blog.Content;
            blog.Slug = Input.Blog.Title.Slugify();
            var tags = Tag.ParseTags(Input.Tags);

            if (Input.UploadCoverPhoto != null)
            {
                var image = Input.UploadCoverPhoto;
                var fileName = $"{blog.Id}_cover.jpg";
                var fileNameAbsPath = Path.Combine(_env.WebRootPath, "db_files", "img", fileName);
                ImageHelper.ResizeToRectangle(image.OpenReadStream(), fileNameAbsPath);
                blog.CoverPhotoPath = $"/db_files/img/{fileName}";
            }

            await _blogRepository.UpdateTagsAsync(blog, false, tags);
            await _blogRepository.UpdateBlogAsync(blog);
            return RedirectToPage("/Blog/Index", new { slug = blog.Slug });
        }
    }
}