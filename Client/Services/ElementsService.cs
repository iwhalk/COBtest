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

        public async Task<Element> GetElementAsync(int id)
        {
            return await _repository.GetAsync<Element>(id, path: "api/Elements");
        }

        public async Task<List<Element>> GetElementsAsync(int? idActivity = null)
        {
            Dictionary<string, string> parameters = new();

            if (idActivity != null)
            {
                parameters.Add("idActivity", idActivity.ToString());
            }

            return await _repository.GetAsync<List<Element>>(path: "api/Elements", parameters: parameters);
        }

        public async Task<Element> PostElementAsync(Element element)
        {
            return await _repository.PostAsync(element, path: "api/Elements");
        }
    }
}
