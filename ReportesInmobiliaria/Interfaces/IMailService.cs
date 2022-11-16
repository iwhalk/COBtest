using SharedLibrary.Models;

namespace ReportesInmobiliaria.Interfaces
{
    public interface IMailService
    {
        Task<> SendPDF();
    }
}
