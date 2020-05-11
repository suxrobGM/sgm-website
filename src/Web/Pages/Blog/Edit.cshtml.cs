using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SuxrobGM_Website.Data;
using SuxrobGM_Website.Models;
using SuxrobGM_Website.Utils;

namespace SuxrobGM_Website.Pages.Blog
{
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EditModel(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Article Article { get; set; }

        [BindProperty]
        public IFormFile UploadCoverPhoto { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Article = await _context.Articles.FirstAsync(i => i.Id == id);
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

            var article = await _context.Articles.FirstAsync(i => i.Id == Article.Id);
            article.Title = Article.Title;
            article.Summary = Article.Summary;
            article.Content = Article.Content;
            article.Tags = Article.Tags;
            article.Slug = Article.CreateSlug(Article.Title);

            if (UploadCoverPhoto != null)
            {
                var image = UploadCoverPhoto;
                var fileName = $"{article.Id}_cover.jpg";
                var fileNameAbsPath = Path.Combine(_env.WebRootPath, "db_files", "img", fileName);
                ImageHelper.ResizeToRectangle(image.OpenReadStream(), fileNameAbsPath);
                article.CoverPhotoUrl = $"/db_files/img/{fileName}";
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("/Blog/Index", new { slug = article.Slug });
        }
    }
}