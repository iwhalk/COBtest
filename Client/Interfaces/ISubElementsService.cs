using SharedLibrary.Models;
using SharedLibrary;

namespace Obra.Client.Interfaces
{
    public interface ISubElementsService
    {
        Task<List<SubElement>> GetSubElementsAsync(int? idElement = null);
        Task<SubElement> PostSubElementAsync(SubElement subElement);
    }
}
