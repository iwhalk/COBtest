using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;

namespace Obra.Client.Services
{
    public class BlobsService : IBlobsService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public BlobsService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }
        public async Task<Blob> GetBlobAsync(int? id)
        {
            return await _repository.GetAsync<Blob>(id, path: "api/Blob");
        }
        public async Task<List<Blob>> GetBlobsAsync(int? id)
        {
            Dictionary<string, string> parameters = new();

            if (id != null)
            {
                parameters.Add("id", id.ToString());
            }

            return await _repository.GetAsync<List<Blob>>(path: "api/Blobs", parameters: parameters);
        }  
        public async Task<Blob> PostBlobAsync(Blob blob)
        {
            return await _repository.PostAsync(blob);
        }
    }
}
