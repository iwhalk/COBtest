namespace ReportesInmobiliaria.Interfaces
{
    public interface IReporteFeaturesService
    {
        Task<byte[]> GetReporteFeatures(int? id);
    }
}
