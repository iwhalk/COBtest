using Shared.Models;

namespace ReportesInmobiliaria.Interfaces
{
    public interface ILessorService
    {
        Task<List<Lessor?>> GetLessorsAsync();
        Task<Lessor?> CreateLessorAsync(Lessor lessor);
    }
}
