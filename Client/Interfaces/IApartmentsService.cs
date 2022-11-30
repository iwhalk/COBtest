using SharedLibrary.Models;
using SharedLibrary;

namespace Client.Interfaces
{
    public interface IApartmentsService
    {
        Task<List<Apartment>> GetApartmentsAsync();
        Task<Apartment> PostApartmentAsync(Apartment apartment);
    }
}
