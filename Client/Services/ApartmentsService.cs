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

        public async Task<Apartment> GetApartmentAsync(int id)
        {           
            return await _repository.GetAsync<Apartment>(id, path: "api/Apartments");
        }

        public async Task<List<Apartment>> GetApartmentsAsync()
        {
            return await _repository.GetAsync<List<Apartment>>(path: "api/Apartments");            
        }

        public async Task<Apartment> PostApartmentAsync(Apartment apartment)
        {
            return await _repository.PostAsync(apartment, path: "api/Apartments");
        }
    }
}
