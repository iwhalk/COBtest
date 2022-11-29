using Client.Interfaces;
using Client.Stores;
using SharedLibrary.Models;

namespace Client.Services
{
    public class AreasService : IAreaService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public AreasService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<Area>> GetAreaAsync()
        {
            if (_context.Area == null)
            {
                var response = await _repository.GetAsync<List<Area>>("api/Area");

                if (response != null)
                {
                    _context.Area = response;
                    return _context.Area;
                }
            }

            return _context.Area;
        }

        public async Task<List<AreaService>> GetAreaServicesAsync()
        {
            var response = await _repository.GetAsync<List<AreaService>>("api/Area/AreaServices");
            return response;
        }
        public async Task<Area> PostAreaAsync(Area area)
        {
            return await _repository.PostAsync("api/Area", area);
        }
    }
}
