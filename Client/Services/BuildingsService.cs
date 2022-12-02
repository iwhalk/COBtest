using Client.Interfaces;
using Client.Stores;
using SharedLibrary.Models;

namespace Client.Services
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

        public async Task<List<Building>> GetBuildingsAsync()
        {
            if (_context.Building == null)
            {
                var response = await _repository.GetAsync<List<Building>>("api/Buildings");

                if (response != null)
                {
                    _context.Building = response;
                    return _context.Building;
                }
            }

            return _context.Building;
        }

        public async Task<Building> PostBuildingAsync(Building building)
        {
            return await _repository.PostAsync("api/Buildings", building);
        }
    }
}
