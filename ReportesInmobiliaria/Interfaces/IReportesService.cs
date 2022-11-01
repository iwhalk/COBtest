namespace ReportesInmobiliaria.Interfaces
{
    public interface IReportesService
    {
        Task<byte[]> GetReporteArrendadores(int? id);
    }
}
