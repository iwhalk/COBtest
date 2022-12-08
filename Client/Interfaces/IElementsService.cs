using SharedLibrary.Models;
using SharedLibrary;

namespace Obra.Client.Interfaces
{
    public interface IElementsService
    {
        Task<Element> GetElementAsync(int id);
        Task<List<Element>> GetElementsAsync(int? idActivity = null);
        Task<Element> PostElementAsync(Element element);
    }
}
