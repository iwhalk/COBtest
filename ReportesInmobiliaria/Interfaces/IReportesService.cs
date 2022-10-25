using Shared.Models;

namespace ReportesInmobiliaria.Interfaces
{
    public interface IReportesService
    {
        Task<byte[]> GetReporteTransaccionesCrucesTotales(string? dia, string? mes, string? semana, string? tag, string? placa, string? noEconomico);
        Task<byte[]> GetReporteCrucesFerromex(string? dia, string? mes, string? semana, string? tag, string? placa, string? noEconomico);
        Task<byte[]> GetReporteCrucesFerromexResumen(string? dia, string? mes, string? semana, string? tag, string? placa, string? noEconomico);
        Task<byte[]> GetReporteIngresosResumen(string? dia, string? mes, string? semana);
        Task<byte[]> GetReporteTransaccionesTurno(string? carril, string? fecha);
        Task<byte[]> GetReporteConcentradoTurno(string? carril, string? fecha);
        Task<byte[]> GetReporteMantenimientoTags(string? tag, bool? estatus, string? fecha, string? noDePlaca, string? noEconomico);
        Task<byte[]> GetReportesActividadUsuarios(string? dia, string? semana, string? mes, string? nombre, string? rol, string? accion);
        Task<List<ActividadUsuarios>> GetActividadUsuarios(Fechas fechas, string? nombre, string? rol, string? accion);

    }
}
