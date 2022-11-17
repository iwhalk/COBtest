using SharedLibrary.Models;

namespace FrontEnd.Interfaces
{
    public interface IUserService
    {
        Task<List<AspNetUser>> GetUsersAsync();
    }
}
