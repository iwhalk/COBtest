using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;

namespace Obra.Client.Services
{
    public class AreasService : IAreasService
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
            Dictionary<string, string> parameters = new();

            if (id != null && id > 0)
            {
                parameters.Add("id", id.ToString());
            }

            return await _repository.GetAsync<Area>(id, parameters: parameters, path: "api/Areas");
        }

        public async Task<List<Area>> GetAreasAsync()
        {
            if (_context.Area == null)
            {
                var response = await _repository.GetAsync<List<Area>>(path: "api/Areas");

                if (response != null)
                {
                    _context.Area = response;
                    return _context.Area;
                }
            }

            return _context.Area;
        }

        public async Task<Area> PostAreaAsync(Area area)
        {
            return await _repository.PostAsync(area, path: "api/Areas");
        }
    }
}
