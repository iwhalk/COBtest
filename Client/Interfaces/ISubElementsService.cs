using SharedLibrary.Models;
using SharedLibrary;

namespace Obra.Client.Interfaces
{
    public interface ISubElementsService
    {
        Task<SubElement> GetSubElementAsync(int id);
        Task<List<SubElement>> GetSubElementsAsync(int? idElement = null);
        Task<SubElement> PostSubElementAsync(SubElement subElement);
    }
}
