using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IElementsService
    {
        Task<List<Element>?> GetElementsAsync();
        Task<Element?> GetElementAsync(int id);
        Task<Element?> CreateElementAsync(Element element);
        Task<bool> UpdateElementAsync(Element element);
        Task<bool> DeleteElementAsync(int id);
    }
}
