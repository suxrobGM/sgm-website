using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using SGM.Domain.Entities.UserEntities;
using SGM.Web.Blog.Utils;

namespace SGM.Web.Blog.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ImageHelper _imageHelper;
        private readonly IEmailSender _emailSender;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ImageHelper imageHelper,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _imageHelper = imageHelper;
        }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string ProfilePhotoUrl { get; set; }
            public IFormFile UploadPhoto { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            
            IsEmailConfirmed = user.EmailConfirmed;

            Input = new InputModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePhotoUrl = user.ProfilePhotoPath
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }           

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            user.UserName = Input.Username;
            user.Email = Input.Email;
            user.PhoneNumber = Input.PhoneNumber;
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;

            if (Input.UploadPhoto != null)
            {
                user.ProfilePhotoPath = _imageHelper.UploadImage(Input.UploadPhoto, $"{user.Id}_profile", true);
            }

            var result = await _userManager.UpdateAsync(user);

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";

            return Page();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                null,
                new { userId = user.Id, code },
                Request.Scheme);

            await _emailSender.SendEmailAsync(
                user.Email,
                "Confirm your email suxrobgm.net",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}
