using SharedLibrary.Models;

namespace FrontEnd.Interfaces
{
    public interface IServicesService
    {
        Task<List<Service>> GetServicesAsync();
        Task<Service> PostServicesAsync(Service service);
    }
}
