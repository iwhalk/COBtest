using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface ISubElementsService
    {
        Task<ApiResponse<SubElement>> GetSubElementAsync(int id);
        Task<ApiResponse<List<SubElement>>> GetSubElementsAsync(int? ID_Element);
        Task<ApiResponse<SubElement>> PostSubElementAsync(SubElement subElement);
    }
}
