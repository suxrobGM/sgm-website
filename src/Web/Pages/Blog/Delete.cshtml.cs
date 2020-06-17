using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SuxrobGM_Website.Data;
using SuxrobGM_Website.Models;
using SuxrobGM_Website.Utils;

namespace SuxrobGM_Website.Pages.Blog
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DeleteModel(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Article Article { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Article = await _context.Articles
                .Include(b => b.Author).FirstOrDefaultAsync(m => m.Id == id);

            if (Article == null)
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

            Article = await _context.Articles.FindAsync(id);

            if (Article != null)
            {
                // remove comments before deleting article
                foreach (var comment in Article.Comments)
                {
                    await RemoveChildrenCommentsAsync(comment);
                    _context.Comments.Remove(comment);
                }

                _context.Articles.Remove(Article);
                var imgFileName = Path.GetFileName(Article.CoverPhotoUrl);
                var imgFullPath = Path.Combine(_env.WebRootPath, "db_files", "img", imgFileName);
                ImageHelper.DeleteImage(imgFullPath);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Blog/List");
        }

        private async Task RemoveChildrenCommentsAsync(Comment rootComment)
        {
            foreach (var reply in rootComment.Replies)
            {
                await RemoveChildrenCommentsAsync(reply);
                _context.Comments.Remove(reply);
            }
        }
    }
}
