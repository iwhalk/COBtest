using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface ISubElementsService
    {
        Task<ApiResponse<List<SubElement>>> GetSubElementsAsync();
        Task<ApiResponse<SubElement>> PostSubElementAsync(SubElement subElement);
    }
}
