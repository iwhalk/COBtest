using FrontEnd.Interfaces;
using FrontEnd.Stores;
using SharedLibrary.Models;

namespace FrontEnd.Services
{
    public class MailAriService : IMailAriService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public MailAriService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<ActasRecepcion>> GetMailAsync(int? idReceptionCertificate, string? email)
        {
            string idReceptionCertificateS = null;

            if (idReceptionCertificate is not null && idReceptionCertificate > 0)
            {
                idReceptionCertificateS = idReceptionCertificate.ToString();
            }

            return await _repository.GetAsync<List<ActasRecepcion>>($"api/MailAri?idReceptionCertificate={idReceptionCertificateS}&email={email}");
        }
    }
}
