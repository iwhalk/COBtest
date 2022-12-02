using Client.Interfaces;
using Client.Stores;
using SharedLibrary.Models;

namespace Client.Services
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

        public async Task<List<Area>> GetAreasAsync()
        {
            if (_context.Area == null)
            {
                var response = await _repository.GetAsync<List<Area>>("api/Areas");

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
            return await _repository.PostAsync("api/Areas", area);
        }
    }
}
