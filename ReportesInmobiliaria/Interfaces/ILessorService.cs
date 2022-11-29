using SharedLibrary.Models;

namespace ReportesObra.Interfaces
{
    public interface ILessorService
    {
        Task<List<Lessor?>> GetLessorsAsync();
        Task<Lessor?> CreateLessorAsync(Lessor lessor);
    }
}
