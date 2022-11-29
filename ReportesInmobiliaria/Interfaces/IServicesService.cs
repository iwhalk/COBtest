using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IServicesService
    {
        Task<List<Service?>> GetServicesAsync();
        Task<Service?> CreateServicesAsync(Service service);
    }
}