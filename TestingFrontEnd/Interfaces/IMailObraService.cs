using SharedLibrary.Models;

namespace FrontEnd.Interfaces
{
    public interface IMailObraService
    {
        Task<List<ActasRecepcion>> GetMailAsync(int? idReceptionCertificate, string? email);
    }
}
