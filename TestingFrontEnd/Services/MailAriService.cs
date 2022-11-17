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

        public async Task<List<ActasRecepcion>> GetMailAsync(int? idProperty, string? email)
        {
            string idPropertyS = null;

            if (idProperty is not null && idProperty > 0)
            {
                idPropertyS = idProperty.ToString();
            }

            return await _repository.GetAsync<List<ActasRecepcion>>($"api/MailAri?idProperty={idPropertyS}&email={email}");
        }
    }
}
