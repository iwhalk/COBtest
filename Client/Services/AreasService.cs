using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;

namespace Obra.Client.Services
{
    public class AreasService : IActivityService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public AreasService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<Area> GetAreaAsync(int id)
        {
            return await _repository.GetAsync<Area>(id, path: "api/Areas");
        }

        public async Task<List<Area>> GetAreasAsync()
        {
            return await _repository.GetAsync<List<Area>>(path: "api/Areas");
        }
        public async Task<Area> PostAreaAsync(Area area)
        {
            return await _repository.PostAsync(area, path: "api/Areas");
        }
    }
}
