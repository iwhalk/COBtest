using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface ISubElementsService
    {
        Task<List<SubElement>?> GetSubElementsAsync();
        Task<SubElement?> GetSubElementAsync(int id);
        Task<SubElement?> CreateSubElementAsync(SubElement subElement);
        Task<bool> UpdateSubElementAsync(SubElement subElement);
        Task<bool> DeleteSubElementAsync(int id);
    }
}
