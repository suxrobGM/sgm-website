using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SuxrobGM_Resume.Data;
using SuxrobGM_Resume.Models;
using SuxrobGM_Resume.Utils;

namespace SuxrobGM_Resume.Pages.Blogs
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
            Blog = _context.Blogs.Where(i => i.Id == blogId).First();
            return Page();
        }

        [BindProperty]
        public Blog Blog { get; set; }

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

            var blog = _context.Blogs.Where(i => i.Id == blogId).First();
            blog.Title = Blog.Title;
            blog.Summary = Blog.Summary;
            blog.Content = Blog.Content;
            blog.Tags = Blog.Tags;

            if (UploadCoverPhoto != null)
            {
                var image = UploadCoverPhoto;
                var fileName = $"{blog.Id}_cover.jpg";
                var fileNameAbsPath = Path.Combine(_env.WebRootPath, "db_files", "img", fileName);
                ImageHelper.ResizeToRectangle(image.OpenReadStream(), fileNameAbsPath);
                blog.CoverPhotoUrl = $"/db_files/img/{fileName}";
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/Blogs/List");
        }
    }
}