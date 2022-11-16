using FrontEnd.Interfaces;
using FrontEnd.Stores;
using SharedLibrary.Models;

namespace FrontEnd.Services
{
    public class BlobsInventoryService : IBlobsInventoryService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;

        public BlobsInventoryService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<BlobsInventory>> GetBlobsInventoryAsync()
        {
            return await _repository.GetAsync<List<BlobsInventory>>("api/BlobInventory");
        }

        public async Task<BlobsInventory> PostBlobsInventoryAsync(BlobsInventory blobsInventory)
        {
            return await _repository.PostAsync("api/BlobInventory", blobsInventory);
        }
    }
}
