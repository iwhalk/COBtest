using Shared.Models;
using FrontEnd.Interfaces;

namespace FrontEnd.Services
{
    public class AreaService : IAreaService
    {
        private readonly IGenericRepository _repository;
        public AreaService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Area>> GetAreaAsync()
        {
            return await _repository.GetAsync<List<Area>>("api/Area");
        }

        public async Task<Area> PostAreaAsync(Area area)
        {
            return await _repository.PostAsync<Area>("api/Area", area);
        }
    }
}
