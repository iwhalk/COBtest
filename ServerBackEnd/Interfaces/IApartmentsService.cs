using SharedLibrary;
using SharedLibrary.Models;

namespace ApiGateway.Interfaces
{
    public interface IApartmentsService
    {
        Task<ApiResponse<Apartment>> GetApartmentAsync(int id);
        Task<ApiResponse<List<Apartment>>> GetApartmentsAsync();
        Task<ApiResponse<Apartment>> PostApartmentAsync(Apartment apartment);
    }
}
