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

        public async Task<List<SubElement>> GetSubElementsAsync(int? idElement = null)
        {
            Dictionary<string, string> parameters = new();

            if (idElement != null)
            {
                parameters.Add("idElement", idElement.ToString());
            }

            //if (_context.SubElement == null)
            //{
            //    var response = await _repository.GetAsync<List<SubElement>>(path: "api/SubElements");

            //    if (response != null)
            //    {
            //        _context.SubElement = response;
            //        return _context.SubElement;
            //    }
            //}

            return await _repository.GetAsync<List<SubElement>>(parameters, "api/SubElements");
        }

        public async Task<SubElement> PostSubElementAsync(SubElement subElement)
        {
            return await _repository.PostAsync(subElement, path: "api/SubElements");
        }
    }
}
