using FrontEnd.Interfaces;
using Shared.Models;

namespace FrontEnd.Services
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
            return await _repository.PostAsync("api/Inventory", inventory);
        }
    }
}
