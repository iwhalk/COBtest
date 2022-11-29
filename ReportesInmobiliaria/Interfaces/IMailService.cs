namespace ReportesObra.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendReceptionCertificate(byte[] reporte, string email);
    }
}
