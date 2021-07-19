using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SGM.Domain.Interfaces.Services;

namespace SGM.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<IndexModel> _logger;
        
        public IndexModel(IEmailSender emailSender, 
            ILogger<IndexModel> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }
        
        public class EmailInputModel
        {
            [Required]
            public string Name { get; init; }
            
            [Required]
            public string Subject { get; init; }
            
            [Required, EmailAddress]
            public string Email { get; init; }
            
            [Required]
            public string Message { get; init; }
        }

        [BindProperty]
        public EmailInputModel EmailInput { get; init; }
        
        [TempData]
        public string EmailStatusMessage { get; set; }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostSendEmailAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var message = @$"<p><b>{EmailInput.Name}</b> - {EmailInput.Email}</p>
                            <p>{EmailInput.Message}</p>";
            try
            {
                await _emailSender.SendEmailAsync("suxrobgm@gmail.com", EmailInput.Subject, message);
                EmailStatusMessage = "Your message has been sent successfully";
            }
            catch (Exception e)
            {
                EmailStatusMessage = "Error: could not send email";
                _logger?.LogError("Could not send email, thrown exception: {Exception}", e);
                return RedirectToPage("", "", "email");
            }

            return Page();
        }
    }
}