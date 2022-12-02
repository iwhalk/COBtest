using ApiGateway.Data;
using MimeKit;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IMailObraService
    {
        Task<ApiResponse> GetMailAsync(int idReceptionCertificate, string email);
        bool MailSender(MimeMessage email);
    }
}
