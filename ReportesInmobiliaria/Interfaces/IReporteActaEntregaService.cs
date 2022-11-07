namespace ReportesInmobiliaria.Interfaces
{
    public interface IReporteActaEntregaService
    {
        Task<byte[]> GetActaEntrega(int idProperty, int idTenant, int idContrato, int idUser);
    }
}
