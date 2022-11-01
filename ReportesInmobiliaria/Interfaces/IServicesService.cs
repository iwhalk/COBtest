using Shared.Models;

namespace ReportesInmobiliaria.Interfaces
{
    public interface IServicesService
    {
        Task<List<Service?>> GetServicesAsync();
        Task<Service?> CreateServicesAsync(Service service);
    }
}