using SharedLibrary.Models;
using SharedLibrary;

namespace Obra.Client.Interfaces
{
    public interface IApartmentsService
    {
        Task<Apartment> GetApartmentAsync(int id);
        Task<List<Apartment>> GetApartmentsAsync();
        Task<Apartment> PostApartmentAsync(Apartment apartment);
    }
}