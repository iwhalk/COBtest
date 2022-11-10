using FrontEnd.Stores;
using Shared.Models;
using TestingFrontEnd.Interfaces;

namespace FrontEnd.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public InventoryService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<Inventory>> GetInventoryAsync()
        {
            if (_context.Inventory == null)
            {
                var response = await _repository.GetAsync<List<Inventory>>("api/Inventory");

                if (response != null)
                {
                    _context.Inventory = response;
                    return _context.Inventory;
                }
            }

            return _context.Inventory;
        }

        public async Task<Inventory> PostInventoryAsync(Inventory inventory)
        {
            return await _repository.PostAsync<Inventory>("api/Inventory", inventory);
        }
    }
}
