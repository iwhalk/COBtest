using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;

namespace Obra.Client.Services
{
    public class ElementsService : IElementsService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public ElementsService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<Element>> GetElementsAsync()
        {
            if (_context.Element == null)
            {
                var response = await _repository.GetAsync<List<Element>>(path: "api/Elements");

                if (response != null)
                {
                    _context.Element = response;
                    return _context.Element;
                }
            }

            return _context.Element;
        }

        public async Task<Element> PostElementAsync(Element element)
        {
            return await _repository.PostAsync(element, path: "api/Elements");
        }
    }
}
