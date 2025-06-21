﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SGM.Application;
using SGM.BlogApp.Utils;
using SGM.Domain.Entities.Blogs;
using SGM.Domain.Repositories;

namespace SGM.BlogApp.Pages;

[Authorize(Roles = "SuperAdmin,Admin,Editor")]
public class EditBlogModel : PageModel
{
    private readonly ImageHelper _imageHelper;
    private readonly IBlogRepository _blogRepository;

    public EditBlogModel(ImageHelper imageHelper, IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
        _imageHelper = imageHelper;
    }

    public class InputModel
    {
        public Domain.Entities.Blogs.Blog Blog { get; set; }
        public IFormFile UploadCoverPhoto { get; set; }
        public string Tags { get; set; }
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        var blog = await _blogRepository.GetByIdAsync(id);

        Input = new InputModel()
        {
            Blog = blog,
            Tags = Tag.ConvertTagsToString(blog.Tags)
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

        var blog = await _blogRepository.GetByIdAsync(Input.Blog.Id);
        blog.Title = Input.Blog.Title;
        blog.Summary = Input.Blog.Summary;
        blog.Content = Input.Blog.Content;
        blog.Slug = Input.Blog.Title.Slugify();
        var tags = Tag.ParseTags(Input.Tags);

        if (Input.UploadCoverPhoto != null)
        {
            Input.Blog.CoverPhotoPath = _imageHelper.UploadImage(Input.UploadCoverPhoto, $"{blog.Id}_blog_cover", resizeToRectangle: true);
        }

        await _blogRepository.UpdateTagsAsync(blog, tags);
        await _blogRepository.UpdateBlogAsync(blog);
        return RedirectToPage("/Blog/Index", new { slug = blog.Slug });
    }
}
