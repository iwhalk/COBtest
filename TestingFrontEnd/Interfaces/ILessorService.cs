using SharedLibrary.Models;

namespace FrontEnd.Interfaces
{
    public interface ILessorService
    {
        Task<List<Lessor>> GetLessorAsync();
        Task<Lessor> PostLessorAsync(Lessor lessor);
    }
}
