using ApiGateway.Data;
using MimeKit;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IMailAriService
    {
        Task<ApiResponse> GetMailAsync(int idReceptionCertificate, string email);
        bool MailSender(MimeMessage email);
    }
}
