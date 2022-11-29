namespace ReportesObra.Interfaces
{
    public interface IReporteActaEntregaService
    {
        Task<byte[]> GetActaEntrega(int idReceptionCertificate);
    }
}
