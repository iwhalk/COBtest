using SharedLibrary;
using SharedLibrary.Models;

namespace ApiGateway.Interfaces
{
    public interface IApartmentsService
    {
        Task<ApiResponse<List<Apartment>>> GetApartmentsAsync();
        Task<ApiResponse<Apartment>> PostApartmentAsync(Apartment apartment);
    }
}
