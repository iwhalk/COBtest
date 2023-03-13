using Obra.Client.Interfaces;
using SharedLibrary.Models;

namespace Obra.Client.Services
{
    public class StatusesService : IStatusesService
    {
        private readonly IGenericRepository _repository;

        public StatusesService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Status>> GetStatusesAsync()
        {
            return await _repository.GetAsync<List<Status>>(path: "api/Status");
        }
        public async Task<Status> GetStatusAync(int idStatus)
        {
            return await _repository.GetAsync<Status>(idStatus, path: "api/Status");
        }
    }
}
