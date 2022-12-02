using FrontEnd.Interfaces;
using SharedLibrary.Models;
using FrontEnd.Interfaces;
using FrontEnd.Stores;

namespace FrontEnd.Services
{
    public class DescriptionsService : IDescriptionsService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public DescriptionsService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }
        public async Task<List<Description>> GetDescriptionAsync()
        {
            if (_context.DescriptionList == null)
            {
                var response = await _repository.GetAsync<List<Description>>("api/Description");

                if (response != null)
                {
                    _context.DescriptionList = response;
                    return _context.DescriptionList;
                }
            }

            return _context.DescriptionList;
        }

        public async Task<Description> PostDescriptionAsync(Description description)
        {
            return await _repository.PostAsync("api/Description", description);
        }
    }
}
