using System.Threading.Tasks;

namespace WebApp.Services
{
    public interface IEmailService
    {
        Task sendAsync(string fromEmail, string ToEmail, string subject, string body);
    }
}