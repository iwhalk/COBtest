using Shared;
using Shared.Models;
using SharedLibrary.Models;

namespace ApiGateway.Interfaces
{
    public interface ILessorService
    {
        Task<ApiResponse<List<Lessor>>> GetLessorAsync();
        Task<ApiResponse<Lessor>> PostLessorAsync(Lessor lessor);
    }
}
