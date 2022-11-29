using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface IAspNetUserService
    {
        Task<List<AspNetUser>?> GetAspNetUsersAsync();
    }
}
