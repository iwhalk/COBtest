using SharedLibrary.Models;
using SharedLibrary;

namespace Obra.Client.Interfaces
{
    public interface IApartmentsService
    {
        Task<List<Apartment>> GetApartmentsAsync();
        Task<Apartment> PostApartmentAsync(Apartment apartment);
    }
}
