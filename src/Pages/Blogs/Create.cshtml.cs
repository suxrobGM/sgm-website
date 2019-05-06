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
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SuxrobGM_Resume.Data;
using SuxrobGM_Resume.Models;
using SuxrobGM_Resume.Utils;

namespace SuxrobGM_Resume.Pages.Blogs
{
    [Authorize(Roles = "SuperAdmin,Admin")]
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
        public Blog Blog { get; set; }      

        [BindProperty]
        public IFormFile UploadCoverPhoto { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var currentUser = _context.Users.Where(i => i.UserName == User.Identity.Name).First();
            Blog.Url = "/Blogs/" + Blog.Url.Trim().Replace(" ", "-");
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
                Blog.CoverPhotoUrl = $"/db_files/img/{fileName}";
            }

            if (_context.Blogs.Where(i => i.Url == Blog.Url).Any())
            {
                ModelState.AddModelError("Blog.Url", "This blog url exists please change it");
                return Page();
            }
           
            _context.Blogs.Add(Blog);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Blogs/List");
        }
    }
}