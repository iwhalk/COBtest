using Client.Interfaces;
using Client.Stores;
using SharedLibrary.Models;
using System.Xml.Linq;

namespace Client.Services
{
    public class SubElementsService : ISubElementsService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public SubElementsService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<SubElement>> GetSubElementsAsync()
        {
            if (_context.SubElement == null)
            {
                var response = await _repository.GetAsync<List<SubElement>>("api/SubElements");

                if (response != null)
                {
                    _context.SubElement = response;
                    return _context.SubElement;
                }
            }

            return _context.SubElement;
        }

        public async Task<SubElement> PostSubElementAsync(SubElement subElement)
        {
            return await _repository.PostAsync("api/SubElements", subElement);
        }
    }
}
