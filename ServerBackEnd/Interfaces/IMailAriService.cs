using ApiGateway.Data;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IMailAriService
    {
        Task<ApiResponse> GetMailAsync(int idProperty, string email);
        bool MailSender(Email email);
    }
}
