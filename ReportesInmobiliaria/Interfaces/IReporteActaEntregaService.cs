namespace ReportesInmobiliaria.Interfaces
{
    public interface IReporteActaEntregaService
    {
        Task<byte[]> GetActaEntrega(int idReceptionCertificate);
    }
}
