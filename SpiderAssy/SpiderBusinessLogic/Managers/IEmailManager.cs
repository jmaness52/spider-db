using System.Threading.Tasks;

namespace SpiderBusinessLogic.Managers
{
    public interface IEmailManager
    {
        Task SendConfirmationEmail(string callbackUrl, string userEmail);
    }
}