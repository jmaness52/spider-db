using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpiderUI.Email;

namespace SpiderUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        private AuthMessageSenderOptions _options;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IConfiguration configuration,
            IOptions<AuthMessageSenderOptions> options)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _configuration = configuration;
            _options = options.Value;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await SendConfirmationEmail(callbackUrl, Input.Email);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        /// <summary>
        /// Builds a message for an administrator to confirm the creation of a new user
        /// </summary>
        /// <param name="userEmail">newly created user to confirm</param>
        /// <param name="callbackUrl">a url for the admin to click on and generate a code</param>
        /// <returns>a string containing the email message to send</returns>
        private string BuildConfirmationEmail(string userEmail, string callbackUrl)
        {
            //Build up an email from the messages in the appsettings.json file
            StringBuilder sb = new StringBuilder();

            sb.Append("<p>");

            //A user is requesting access to the database system. Email Address: userEmail
            sb.Append(_configuration["ConfirmationEmail:Message1"] + " " + userEmail);

            sb.Append("</p><p>");

            //Message2 contains a token "{urlToken}" that is replaced with the callback url.
            string confirmLink = _configuration["ConfirmationEmail:Message2"].Replace("{urlToken}", HtmlEncoder.Default.Encode(callbackUrl));
            sb.Append(confirmLink);

            sb.Append("</p>");

            return sb.ToString();
        }
        /// <summary>
        /// Sends a confirmation email to the administrator when a new user registers
        /// </summary>
        /// <param name="callbackUrl">URL for admin to confirn new user</param>
        /// <param name="userEmail">new user's email address</param>
        private async Task SendConfirmationEmail(string callbackUrl, string userEmail)
        {
            string message = BuildConfirmationEmail(userEmail, callbackUrl);

            await _emailSender.SendEmailAsync(_options.AdminEmailAddress, _configuration["ConfirmationEmail:Subject"], message);
        }
    }
}
