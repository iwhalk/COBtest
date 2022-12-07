using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;
using System.Diagnostics;
using System.Xml.Linq;

namespace Obra.Client.Services
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

        public async Task<SubElement> GetSubElementAsync(int id)
        {
            return await _repository.GetAsync<SubElement>(id, path: "api/SubElements");
        }

        public async Task<List<SubElement>> GetSubElementsAsync(int? idElement = null)
        {
            Dictionary<string, string> parameters = new();

            if (idElement != null)
            {
                parameters.Add("ID_Element", idElement.ToString());
            }

            return await _repository.GetAsync<List<SubElement>>(path: "api/SubElements", parameters: parameters);
        }

        public async Task<SubElement> PostSubElementAsync(SubElement subElement)
        {
            return await _repository.PostAsync(subElement, path: "api/SubElements");
        }
    }
}
