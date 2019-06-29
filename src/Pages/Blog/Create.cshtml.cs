using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM_Resume.Data;
using SuxrobGM_Resume.Models;
using SuxrobGM_Resume.Utils;

namespace SuxrobGM_Resume.Pages.Blog
{
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _env;

        public CreateModel(ApplicationDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult OnGet()
        {
            //ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "UserName");
            return Page();
        }

        [BindProperty]
        public Article Article { get; set; }      

        [BindProperty]
        public IFormFile UploadCoverPhoto { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var currentUser = _context.Users.Where(i => i.UserName == User.Identity.Name).First();
            Article.Url = "/Blog/" + Article.Url.Trim().Replace(" ", "-");
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
                Article.CoverPhotoUrl = $"/db_files/img/{fileName}";
            }

            if (_context.Articles.Where(i => i.Url == Article.Url).Any())
            {
                ModelState.AddModelError("Blog.Url", "This blog url exists please change it");
                return Page();
            }
           
            _context.Articles.Add(Article);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Blog/List");
        }
    }
}