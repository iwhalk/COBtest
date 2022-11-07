using Shared.Models;

namespace TestingFrontEnd.Interfaces
{
    public interface ILessorService
    {
        Task<List<Lessor>> GetLessorAsync();
        Task<Lessor> PostLessorAsync(Lessor lessor);
    }
}
