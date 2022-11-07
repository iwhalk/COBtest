using Shared.Models;

namespace TestingFrontEnd.Interfaces
{
    public interface IServicesService
    {
        Task<List<Service>> GetServicesAsync();
        Task<Service> PostServicesAsync(Service service);
    }
}
