using SharedLibrary.Models;

namespace Obra.Client.Interfaces
{
    public interface IObjectAccessService
    {
        Task<ObjectAccessUser> GetObjectAccess();
    }
}
