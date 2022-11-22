namespace ReportesInmobiliaria.Interfaces
{
    public interface IReporteActaEntregaService
    {
        Task<byte[]> GetActaEntrega(string idProperty);
    }
}
