using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM_Resume.Data;
using SuxrobGM_Resume.Models;
using SuxrobGM_Resume.Utils;

namespace SuxrobGM_Resume.Pages.Blog
{
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _env;

        public EditModel(ApplicationDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult OnGet()
        {
            var blogId = RouteData.Values["id"].ToString();
            Article = _context.Articles.Where(i => i.Id == blogId).First();
            return Page();
        }

        [BindProperty]
        public Article Article { get; set; }

        [BindProperty]
        public IFormFile UploadCoverPhoto { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var blogId = RouteData.Values["id"].ToString();
            ModelState.Values.First().ValidationState = ModelValidationState.Valid;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var article = _context.Articles.Where(i => i.Id == blogId).First();
            article.Title = Article.Title;
            article.Summary = Article.Summary;
            article.Content = Article.Content;
            article.Tags = Article.Tags;

            if (UploadCoverPhoto != null)
            {
                var image = UploadCoverPhoto;
                var fileName = $"{article.Id}_cover.jpg";
                var fileNameAbsPath = Path.Combine(_env.WebRootPath, "db_files", "img", fileName);
                ImageHelper.ResizeToRectangle(image.OpenReadStream(), fileNameAbsPath);
                article.CoverPhotoUrl = $"/db_files/img/{fileName}";
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/Blog/List");
        }
    }
}