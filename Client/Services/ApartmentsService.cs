using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;
using System.Diagnostics;

namespace Obra.Client.Services
{
    public class ApartmentsService : IApartmentsService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public ApartmentsService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<Apartment>> GetApartmentsAsync()
        {
            if (_context.Apartment == null)
            {
                var response = await _repository.GetAsync<List<Apartment>>("api/Apartments");

                if (response != null)
                {
                    _context.Apartment = response;
                    return _context.Apartment;
                }
            }

            return _context.Apartment;
        }

        public async Task<Apartment> PostApartmentAsync(Apartment apartment)
        {
            return null;
            //return await _repository.PostAsync("api/Apartments", apartment);
        }
    }
}
