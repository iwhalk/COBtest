using SharedLibrary.Models;
namespace ReportesObra.Interfaces
{
    public interface IApartmentsService
    {
        Task<List<Apartment>?> GetApartmentsAsync();
        Task<Apartment?> GetApartmentAsync(int id);
        Task<Apartment?> CreateApartmentAsync(Apartment apartment);
        Task<bool> UpdateApartmentAsync(Apartment apartment);
        Task<bool> DeleteApartmentAsync(int id);
    }
}
