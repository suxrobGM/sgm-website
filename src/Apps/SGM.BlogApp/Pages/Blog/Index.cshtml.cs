﻿using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SGM.Domain.Entities.Blogs;
using SGM.Domain.Entities.Users;
using SGM.Domain.Repositories;
using SuxrobGM.Sdk.AspNetCore.Pagination;

namespace SGM.BlogApp.Pages;

public class BlogIndexModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IBlogRepository _blogRepository;
    private readonly IEmailSender _emailSender;

    public BlogIndexModel(UserManager<ApplicationUser> userManager,
        IBlogRepository blogRepository,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _blogRepository = blogRepository;
        _emailSender = emailSender;
    }
   
    [Required]
    [BindProperty] 
    public string CommentContent { get; set; }

    [BindProperty]
    [Display(Name = "Name:")]
    public string CommentAuthorName { get; set; }

    [BindProperty]
    [Display(Name = "Email:")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address")]
    public string CommentAuthorEmail { get; set; }

    public string Tags { get; set; }
    public int PageIndex { get; set; }
    public Domain.Entities.Blogs.Blog Blog { get; set; }
    public PaginatedList<Comment> Comments { get; set; }
    

    public async Task<IActionResult> OnGetAsync(int pageIndex = 1)
    {
        var blogSlug = RouteData.Values["slug"]?.ToString();
        Blog = await _blogRepository.GetAsync(i => i.Slug == blogSlug);
        
        if (Blog == null)
        {
            return RedirectToPage("./List");
        }
        
        Tags = Tag.ConvertTagsToString(Blog.Tags);

        if (!Request.Headers["User-Agent"].ToString().ToLower().Contains("bot"))
        {
            Blog.ViewCount++;
        }

        await _blogRepository.UpdateBlogAsync(Blog, false);
        Comments = PaginatedList<Comment>.Create(Blog.Comments, pageIndex);
        PageIndex = pageIndex;
        ViewData.Add("PageIndex", PageIndex);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var blogSlug = RouteData.Values["slug"]?.ToString();

        if (!int.TryParse(HttpContext.Request.Query["pageIndex"].ToString(), out var pageNumber))
        {
            pageNumber = 1;
        }

        if (string.IsNullOrWhiteSpace(CommentContent))
        {
            ModelState.AddModelError("CommentContent", "Empty comment content");
            return Page();
        }

        Blog = await _blogRepository.GetAsync(i => i.Slug == blogSlug);
        var comment = new Comment() { Content = CommentContent };

        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            comment.Author = user;
        }
        else
        {               
            comment.AuthorEmail = CommentAuthorEmail;
            comment.AuthorName = CommentAuthorName;
        }

        var htmlMsg = $@"<h3>Good day, <b>{Blog.Author.UserName}</b></h3>
                                <p>Posted comment in your blog in suxrobgm.net <a href='{HtmlEncoder.Default.Encode($"http://suxrobgm.net/{Blog.Slug}?pageIndex={pageNumber}#{comment.Id}")}'>{Blog.Title}</a></p>
                                <br />
                                ";

        await _blogRepository.AddCommentAsync(Blog, comment);
        await _emailSender.SendEmailAsync(Blog.Author.Email, "Posted comment in your blog", htmlMsg);
        return RedirectToPage("", "", new { pageIndex = pageNumber }, comment.Id);
    }

    public async Task<IActionResult> OnPostReplyToCommentAsync(string commentId)
    {
        var blogSlug = RouteData.Values["slug"]?.ToString();
        if (!int.TryParse(HttpContext.Request.Query["pageIndex"].ToString(), out var pageNumber))
        {
            pageNumber = 1;
        }

        var blog = await _blogRepository.GetAsync(i => i.Slug == blogSlug);
        var author = await _userManager.GetUserAsync(User);
        var parentComment = await _blogRepository.GetCommentByIdAsync(commentId);

        if (string.IsNullOrWhiteSpace(CommentContent))
        {
            ModelState.AddModelError("CommentContent", "Empty comment content");
            return Page();
        }
       
        var childComment = new Comment()
        {               
            ParentComment = parentComment,
            Author = author,
            Content = CommentContent,
        };

        var commentAuthor = parentComment.Author == null ? parentComment.AuthorName : parentComment.Author.UserName;
        var commentEmail = parentComment.Author == null ? parentComment.AuthorEmail : parentComment.Author.Email;

        var htmlMsg = $@"<h3>Good day, <b>{commentAuthor}</b></h3>
                                <p>Replied to your comment in this suxrobgm.net blog <a href='{HtmlEncoder.Default.Encode($"http://suxrobgm.net/blog/{blog.Slug}?pageIndex={pageNumber}#{commentId}")}'>{blog.Title}</a></p>
                                <br />
                                ";

        await _blogRepository.AddReplyToCommentAsync(parentComment, childComment);
        await _emailSender.SendEmailAsync(commentEmail, "Replied to your comment", htmlMsg);
        return RedirectToPage("", "", new { pageIndex = pageNumber }, commentId);
    }

    public async Task<IActionResult> OnPostDeleteCommentAsync(string commentId, string rootCommentId)
    {
        if (!int.TryParse(HttpContext.Request.Query["pageIndex"].ToString(), out var pageNumber))
        {
            pageNumber = 1;
        }

        var comment = await _blogRepository.GetCommentByIdAsync(commentId);
        await _blogRepository.DeleteCommentAsync(comment);
        return RedirectToPage("", "", new { pageIndex = pageNumber }, rootCommentId);
    }
}
