using System.Threading.Tasks;

namespace LevelUp.Services
{
    public interface IEmailService
    {
        Task SendRegistrationEmailAsync(string toEmail, string username);
    }
}
