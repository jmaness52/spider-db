using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SpiderBusinessLogic.Email;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SpiderBusinessLogic.Managers
{
    public class EmailManager : IEmailManager
    {

        private IEmailSender _emailSender;
        private IConfiguration _configuration;
        private AuthMessageSenderOptions _options;

        public EmailManager(IEmailSender emailSender, IOptions<AuthMessageSenderOptions> options, IConfiguration configuration)
        {
            _emailSender = emailSender;
            _options = options.Value;
            _configuration = configuration;
        }

        /// <summary>
        /// Sends a confirmation email to the administrator when a new user registers
        /// </summary>
        /// <param name="callbackUrl">URL for admin to confirn new user</param>
        /// <param name="userEmail">new user's email address</param>
        public async Task SendConfirmationEmail(string callbackUrl, string userEmail)
        {
            string message = BuildConfirmationEmail(userEmail, callbackUrl);

            await _emailSender.SendEmailAsync(_options.AdminEmailAddress, _configuration["ConfirmationEmail:Subject"], message);
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

    }
}
