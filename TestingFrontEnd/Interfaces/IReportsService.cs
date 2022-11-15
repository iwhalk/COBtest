using SharedLibrary.Models;

namespace FrontEnd.Interfaces
{
    public interface IReportsService
    {
        Task<byte[]> GetReportFeature(int Id);
    }
}
