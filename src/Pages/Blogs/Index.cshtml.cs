using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM.Sdk.Pagination;
using SuxrobGM_Resume.Data;
using SuxrobGM_Resume.Models;

namespace SuxrobGM_Resume.Pages.Blogs
{
    public class BlogIndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public BlogIndexModel(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }
       
        [Required]
        [BindProperty] 
        public string CommentText { get; set; }

        [BindProperty]
        [Display(Name = "Name:")]
        public string CommentAuthorName { get; set; }

        [BindProperty]
        [Display(Name = "Email:")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address")]
        public string CommentAuthorEmail { get; set; }

        public int PageIndex { get; set; }
        public Blog Blog { get; set; }
        public PaginatedList<Comment> Comments { get; set; }
        public string[] BlogTags { get; set; }

        public void OnGetAsync(int pageIndex = 1)
        {
            string blogUrl = RouteData.Values["blogUrl"].ToString();
            Blog = _context.Blogs.Where(i => i.GetRelativeUrl() == blogUrl).First();
            BlogTags = Blog.Tags.Split(',');
            Comments = PaginatedList<Comment>.Create(Blog.Comments, pageIndex);
            PageIndex = pageIndex;

            ViewData.Add("PageIndex", PageIndex);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var blogUrl = RouteData.Values["blogUrl"].ToString();

            if (!int.TryParse(HttpContext.Request.Query["pageIndex"].ToString(), out int pageNumber))
            {
                pageNumber = 1;
            }

            if (string.IsNullOrWhiteSpace(CommentText))
            {
                ModelState.AddModelError("CommentText", "Empty comment text");
                return Page();
            }
            
            Blog = _context.Blogs.Where(i => i.GetRelativeUrl() == blogUrl).First();
            var comment = new Comment() { Text = CommentText };

            if (User.Identity.IsAuthenticated)
            {
                var user = _context.Users.Where(i => i.UserName == User.Identity.Name).First();
                comment.Author = user;
            }
            else
            {               
                comment.AuthorEmail = CommentAuthorEmail;
                comment.AuthorName = CommentAuthorName;
            }

            string htmlMsg = $@"<h3>Good day, <b>{Blog.Author.UserName}</b></h3>
                                <p>Posted comment in your article in suxrobgm.net <a href='{HtmlEncoder.Default.Encode($"http://suxrobgm.net{Blog.Url}?pageIndex={pageNumber}#{comment.Id}")}'>{Blog.Title}</a></p>
                                <br />
                                <p>Sincerely, <b>SuxrobGM</b></p>
                            ";

            Blog.Comments.Add(comment);
            await _context.SaveChangesAsync();
            await _emailSender.SendEmailAsync(Blog.Author.Email, "Posted comment in your article", htmlMsg);
            return RedirectToPage("", "", new { pageIndex = pageNumber }, comment.Id);
        }

        public async Task<IActionResult> OnPostReplyToCommentAsync(string commentId)
        {
            var blogUrl = RouteData.Values["blogUrl"].ToString();
            if (!int.TryParse(HttpContext.Request.Query["pageIndex"].ToString(), out int pageNumber))
            {
                pageNumber = 1;
            }

            var blog = _context.Blogs.Where(i => i.GetRelativeUrl() == blogUrl).First();
            var author = _context.Users.Where(i => i.UserName == User.Identity.Name).First();
            var comment = _context.Comments.Where(i => i.Id == commentId).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(CommentText))
            {
                ModelState.AddModelError("CommentText", "Empty comment text");
                return Page();
            }
           
            var reply = new Comment()
            {               
                Parent = comment,
                Author = author,
                Text = CommentText,
            };

            string commentAuthor = comment.AuthorId == null ? comment.AuthorName : comment.Author.UserName;
            string commentEmail = comment.AuthorId == null ? comment.AuthorEmail : comment.Author.Email;

            string htmlMsg = $@"<h3>Good day, <b>{commentAuthor}</b></h3>
                                <p>Replied to your comment in this suxrobgm.net article <a href='{HtmlEncoder.Default.Encode($"http://suxrobgm.net{blog.Url}?pageIndex={pageNumber}#{commentId}")}'>{blog.Title}</a></p>
                                <br />
                                <p>Sincerely, <b>SuxrobGM</b></p>
                            ";

            comment.Replies.Add(reply);
            await _context.SaveChangesAsync();
            await _emailSender.SendEmailAsync(commentEmail, "Replied to your comment", htmlMsg);
            return RedirectToPage("", "", new { pageIndex = pageNumber }, commentId);
        }

        public async Task<IActionResult> OnPostDeleteCommentAsync(string commentId, string rootCommentId)
        {
            var blogUrl = RouteData.Values["blogUrl"].ToString();
            var pageNumber = int.Parse(HttpContext.Request.Query["pageIndex"]);
            var comment = _context.Comments.Where(i => i.Id == commentId).FirstOrDefault();
            
            await RemoveChildrenCommentsAsync(comment);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToPage("", "", new { pageIndex = pageNumber }, rootCommentId);
        }       

        private async Task RemoveChildrenCommentsAsync(Comment comment)
        {
            foreach (var reply in comment.Replies)
            {
                await RemoveChildrenCommentsAsync(reply);
                _context.Comments.Remove(reply);
            }
        }
    }
}