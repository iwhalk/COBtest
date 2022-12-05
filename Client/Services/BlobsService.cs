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

        public async Task<List<Blob>> GetBlobsAsync()
        {
            //if (_context.Blob == null)
            //{
            var response = await _repository.GetAsync<List<Blob>>();

            //if (response != null)
            //{
            //_context.Blob = response;
            return response;
            //}
            //}

            //return _context.Blob;
        }

        public async Task<Blob> PostBlobAsync(Blob blob)
        {
            return await _repository.PostAsync(blob);
        }
    }
}
