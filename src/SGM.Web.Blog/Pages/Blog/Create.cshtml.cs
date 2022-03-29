using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SuxrobGM.Sdk.Extensions;
using SGM.BlogApp.Utils;

namespace SGM.BlogApp.Pages;

[Authorize(Roles = "SuperAdmin,Admin,Editor")]
public class CreateBlogModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ImageHelper _imageHelper;
    private readonly IBlogRepository _blogRepository;

    public CreateBlogModel(UserManager<ApplicationUser> userManager,
        ImageHelper imageHelper,
        IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
        _userManager = userManager;
        _imageHelper = imageHelper;
    }

    public IActionResult OnGet()
    {
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

    public class InputModel
    {
        public Domain.Entities.BlogEntities.Blog Blog { get; set; }
        public IFormFile UploadCoverPhoto { get; set; }
        public string Tags { get; set; }
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var currentUser = await _userManager.GetUserAsync(User);
        var tags = Tag.ParseTags(Input.Tags);
        Input.Blog.Slug = Input.Blog.Title.Slugify();
        Input.Blog.Author = currentUser;

        if (Input.UploadCoverPhoto != null)
        {
            Input.Blog.CoverPhotoPath = _imageHelper.UploadImage(Input.UploadCoverPhoto, $"{Input.Blog.Id}_blog_cover", resizeToRectangle: true);
        }

        await _blogRepository.AddBlogAsync(Input.Blog);
        await _blogRepository.UpdateTagsAsync(Input.Blog, tags);
        return RedirectToPage("/Blog/Index", new { slug = Input.Blog.Slug });
    }
}
