using Shared.Models;
using TestingFrontEnd.Interfaces;

namespace TestingFrontEnd.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IGenericRepository _repository;
        public InventoryService(IGenericRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Inventory>> GetInventoryAsync()
        {
            return await _repository.GetAsync<List<Inventory>>("api/Inventory");
        }

        public async Task<Inventory> PostInventoryAsync(Inventory inventory)
        {
            return await _repository.PostAsync<Inventory>("api/Inventory", inventory);
        }
    }
}
