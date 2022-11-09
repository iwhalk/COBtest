using Shared.Models;
using TestingFrontEnd.Interfaces;
using TestingFrontEnd.Stores;

namespace TestingFrontEnd.Services
{
    public class DescriptionService : IDescriptionService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public DescriptionService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }
        public async Task<List<Description>> GetDescriptionAsync()
        {
            if (_context.Description == null)
            {
                var response = await _repository.GetAsync<List<Description>>("api/Description");

                if (response != null)
                {
                    _context.Description = response;
                    return _context.Description;
                }
            }

            return _context.Description;
        }

        public async Task<Description> PostDescriptionAsync(Description description)
        {
            return await _repository.PostAsync<Description>("api/Description", description);
        }
    }
}
