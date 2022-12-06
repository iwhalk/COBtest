using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;

namespace Obra.Client.Services
{
    public class BuildingsService : IBuildingsService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public BuildingsService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<Building> GetBuildingAsync(int id)
        {      
            return await _repository.GetAsync<Building>(id, path: "Building");
        }
        public async Task<List<Building>> GetBuildingsAsync()
        {        
            return await _repository.GetAsync<List<Building>>(path: "api/Buildings");
        }
        public async Task<Building> PostBuildingAsync(Building building)
        {
            return await _repository.PostAsync(building, path: "api/Buildings");
        }
    }
}
