using SharedLibrary.Models;

namespace ReportesInmobiliaria.Interfaces
{
    public interface IAspNetUserService
    {
        Task<List<AspNetUser>?> GetAspNetUsersAsync();
    }
}
