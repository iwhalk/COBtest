using SharedLibrary.Models;
using SharedLibrary;

namespace Obra.Client.Interfaces
{
    public interface IElementsService
    {
        Task<List<Element>> GetElementsAsync(int? idActivity = null);
        Task<Element> PostElementAsync(Element element);
    }
}
