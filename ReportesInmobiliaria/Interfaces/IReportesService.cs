using Shared.Models;

namespace ReportesInmobiliaria.Interfaces
{
    public interface IReportesService
    {
        Task<UsuarioPlaza> GetUsuarioPlazaAsync();
        Task<List<TypeDelegacion>> GetDelegacionesAsync();
        Task<List<TypePlaza>> GetPlazasAsync();
        KeyValuePair<string, string>[] GetTurnos();
        List<Personal> GetAdministradores();
        List<Personal> GetEncargadosTurno();
        Task<List<Bolsa>> GetBolsasCajeroReceptorAsync(CajeroReceptor cajeroReceptor);
        Task<Stream> CreateReporteCajeroReceptorAsync(CajeroReceptor cajeroReceptor);
        Task<Stream> CreateReporteTurnoCarrilesAsync(TurnoCarriles turnoCarriles);
        Task<Stream> CreateReporteDiaCasetaAsync(DiaCaseta diaCaseta);
    }
}
