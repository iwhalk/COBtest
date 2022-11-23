using SharedLibrary.Models;

namespace FrontEnd.Interfaces
{
    public interface IMailAriService
    {
        Task<List<ActasRecepcion>> GetMailAsync(int? idReceptionCertificate, string? email);
    }
}
