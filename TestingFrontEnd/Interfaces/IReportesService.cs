using ReportesData.Models;

namespace TestingFrontEnd.Interfaces
{
    public interface IReportesService
    {
        Task<UsuarioPlaza> GetUsuarioPlazaAsync();
        Task<List<TypeDelegacion>> GetDelegacionesAsync();
        Task<List<TypePlaza>> GetPlazasAsync();
        Task<KeyValuePair<string, string>[]> GetTurnosAsync();
        Task<List<Personal>> GetAdministradoresAsync();
        Task<List<Personal>> GetEncargadosTurnoAsync();
        Task<List<Bolsa>> CreateBolsasCajeroReceptorAsync(CajeroReceptor cajeroReceptor);
        Task<byte[]> CreateReporteCajeroReceptorAsync(CajeroReceptor cajeroReceptor);
        Task<byte[]> CreateReporteTurnoCarrilesAsync(TurnoCarriles turnoCarriles);
        Task<byte[]> CreateReporteDiaCasetaAsync(DiaCaseta diaCaseta);
    }
}
