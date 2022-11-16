using SharedLibrary;
using SharedLibrary.Models;

namespace ApiGateway.Interfaces
{
    public interface IAspNetUserService
    {
        Task<ApiResponse<List<AspNetUser>>> GetAspNetUsersAsync();
    }
}
