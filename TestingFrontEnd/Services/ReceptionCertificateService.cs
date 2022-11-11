using FrontEnd.Interfaces;
using FrontEnd.Stores;
using Shared.Models;
using SharedLibrary.Models;

namespace FrontEnd.Services
{
    public class ReceptionCertificateService : IReceptionCertificateService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;

        public ReceptionCertificateService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<ActasRecepcion>> GetReceptionCertificatesAsync(string? day, string? week, string? month, string? propertyType, string? numberOfRooms, string? lessor, string? tenant, string? delegation, string? agent, string? currentPage, string? rowNumber)
        {
            if (_context.ActasRecepcion == null)
            {
                var response = await _repository.GetAsync<List<ActasRecepcion>>($"api/ReceptionCertificates?day={day}&week={week}&month={month}&propertyType={propertyType}&numberOfRooms={numberOfRooms}&lessor={lessor}&tenant={tenant}&delegation={delegation}&agent={agent}&currentPage={currentPage}&rowNumber={rowNumber}");

                if (response != null)
                {
                    _context.ActasRecepcion = response;
                    return _context.ActasRecepcion;
                }
            }

            return _context.ActasRecepcion;
        }

        public async Task<ReceptionCertificate> PostReceptionCertificatesAsync(ReceptionCertificate receptionCertificate)
        {
            return await _repository.PostAsync("api/ReceptionCertificates", receptionCertificate);
        }
    }
}
