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
            Dictionary<string, string> parameters = new ();

            if (id != null && id > 0)
            {
                parameters.Add("id", id.ToString());
            }

            return await _repository.GetAsync<Building>(id, parameters: parameters, path: "api/Buildings");
        }

        public async Task<List<Building>> GetBuildingsAsync()
        {
            if (_context.Building == null)
            {
                var response = await _repository.GetAsync<List<Building>>(path: "api/Buildings");

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
            return await _repository.PostAsync(building, path: "api/Buildings");
        }
    }
}
