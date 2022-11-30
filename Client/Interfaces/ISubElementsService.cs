using SharedLibrary.Models;
using SharedLibrary;

namespace Client.Interfaces
{
    public interface ISubElementsService
    {
        Task<List<SubElement>> GetSubElementsAsync();
        Task<SubElement> PostSubElementAsync(SubElement subElement);
    }
}
