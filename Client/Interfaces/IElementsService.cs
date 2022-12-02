using SharedLibrary.Models;
using SharedLibrary;

namespace Client.Interfaces
{
    public interface IElementsService
    {
        Task<List<Element>> GetElementsAsync();
        Task<Element> PostElementAsync(Element element);
    }
}
