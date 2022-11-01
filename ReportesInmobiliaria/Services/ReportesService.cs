using Microsoft.EntityFrameworkCore;
using ReportesInmobiliaria.Interfaces;
using ReportesInmobiliaria.Utilities;
using Shared.Data;
using Shared.Models;
using System.Diagnostics;

namespace ReportesInmobiliaria.Services
{
    public class ReportesService : IReportesService
    {
        private readonly InmobiliariaDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ReportesFactory _reportesFactory;

        public ReportesService(InmobiliariaDbContext dbContext, IHttpContextAccessor httpContextAccessor, ReportesFactory reportesFactory)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _reportesFactory = reportesFactory;
        }
        public async Task<byte[]> GetReporteArrendadores(int? id)
        {
            var arrendadores = await _dbContext.Lessors.ToListAsync();

            ReporteArrendadores reporteArrendadores = new()
            {
                FechaGeneracion = DateTime.Now,
                NumeroArrendadores = arrendadores.Count().ToString() ?? "",
                Arrendadores = await GetArrendadoresAsync(id)
            };

            return _reportesFactory.CrearPdf(reporteArrendadores);
        }

        async Task<List<ArrendadoresDetalle>> GetArrendadoresAsync(int? id)
        {
            IQueryable<Lessor> arrendadores = _dbContext.Lessors;

            if (id != null)
                arrendadores = arrendadores.Where(x => x.IdLessor == id);

            var list = new List<ArrendadoresDetalle>();

            foreach (var arrendador in arrendadores)
            {
                var nombreCompleto = arrendador.Name + " " + arrendador.LastName;
                var direccionCompleta = arrendador.Street + ", " + arrendador.Colony + ", " + arrendador.Delegation + ", " + arrendador.Cp;
                list.Add(new ArrendadoresDetalle()
                {
                    Nombre = nombreCompleto != " " ? nombreCompleto : "",
                    RFC = arrendador.Rfc ?? "",
                    Direccion = direccionCompleta,
                    Telefono = arrendador.PhoneNumber ?? ""
                });
            }
            return list;
        }

    }
}


