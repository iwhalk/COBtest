using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Models;
using ReportesInmobiliaria.Interfaces;

namespace ReportesInmobiliaria.Services
{
    public class TelepeajeService : ITelepeajeService
    {
        private readonly AppDbContext _dbContext;

        public TelepeajeService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Obtiene la paginacion de todos los cruces especificados 
        /// </summary>
        /// <param name="paginaActual">Ej. 1</param>
        /// <param name="numeroDeFilas">Ej. 10</param>
        /// <param name="tag">Ej. IMDM22961475</param>
        /// <param name="fecha">Ej. 2022-06-22 06:00:00</param>
        /// <param name="carril">Ej. B06</param>
        /// <param name="cuerpo">Ej. Cuerpo A</param>
        /// <returns>Devuelve la paginacion completa</returns>
        public async Task<List<Cruce>> GetCruces(int? paginaActual, int? numeroDeFilas, string? tag, string? carril, string? cuerpo, DateTime? fecha, string? noDePlaca, string? noEconomico, string? clase)
        {
            var tagsl = _dbContext.TagLists.Where(x => x.Active);

            IQueryable<Transaction>? res = _dbContext.Transactions
                .Include(x => x.IdCatalogNavigation)
                .Include(x => x.IdClass2Navigation)
                .Include(x => x.IdTariffNavigation)
                .Include(x => x.IdPaymentNavigation)
                .Where(x => tagsl.Select(y => y.Tag).Contains(x.IsoContent.Trim())).OrderByDescending(x => x.TransactionDate);

            IQueryable<TagList>? tagLists = _dbContext.TagLists.Where(x => x.Active);

            IQueryable<LaneCatalog>? laneCatalogs = _dbContext.LaneCatalogs;

            if (!string.IsNullOrWhiteSpace(noDePlaca))
            {
                tagLists = tagLists.Where(x => x.VehiclePlate == noDePlaca);
                res = res.Where(x => tagLists.Select(y => y.Tag).Contains(x.IsoContent.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(noEconomico))
            {
                tagLists = tagLists.Where(x => x.EconomicNumber == noEconomico);
                res = res.Where(x => tagLists.Select(y => y.Tag).Contains(x.IsoContent.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(tag))
            {
                res = res.Where(x => x.IsoContent.Trim().Contains(tag.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(carril))
            {
                var lane = await _dbContext.LaneCatalogs.FirstOrDefaultAsync(x => x.IdLane.Equals(carril));
                if (lane != null) res = res.Where(x => x.IdCatalog == lane.IdCatalog);
            }
            if (!string.IsNullOrWhiteSpace(cuerpo))
            {
                var idSides = _dbContext.LaneCatalogs.Where(x => x.Description.Equals(cuerpo) && !string.IsNullOrWhiteSpace(x.IdSide));
                if (idSides != null) res = res.Where(x => idSides.Select(y => y.IdSide).Contains(x.IdCatalogNavigation.IdSide));
            }
            if (fecha != null)
            {
                res = res.Where(x => x.TransactionDate.Date == fecha.Value.Date);
            }
            if (!string.IsNullOrWhiteSpace(clase))
            {
                res = res.Where(x => x.IdClass2Navigation.ClassCode == clase);
            }
            if (paginaActual != null && numeroDeFilas != null)
            {
                res = res.Skip((int)((paginaActual - 1) * numeroDeFilas)).Take((int)numeroDeFilas);
            }

            var transactions = await res.AsNoTracking().ToListAsync();
            var tags = await tagLists.AsNoTracking().Where(x => transactions.Select(y => y.IsoContent.Trim()).Contains(x.Tag)).ToListAsync();
            var lanes = await laneCatalogs.AsNoTracking().ToListAsync();

            List<Cruce> cruces = new();
            foreach (var transaction in transactions)
            {
                cruces.Add(new()
                {
                    Carril = transaction.IdCatalogNavigation?.IdLane,
                    Fecha = transaction.TransactionDate.ToString("yyy-MM-dd"),
                    Hora = transaction.TransactionDate.ToString("hh:mm:ss tt"),
                    Tag = transaction.IsoContent,
                    Clase = transaction.IdClass2Navigation?.ClassCode,
                    Tarifa = transaction.IdTariffNavigation?.Tariff1.ToString("C"),
                    NoEconomico = tags.FirstOrDefault(x => x.Tag == transaction.IsoContent.Trim())?.EconomicNumber,
                    NoPlaca = tags.FirstOrDefault(x => x.Tag == transaction.IsoContent.Trim())?.VehiclePlate,
                    Cuerpo = transaction.IdCatalogNavigation?.IdSide != null ? lanes.FirstOrDefault(x => x.IdSide == transaction.IdCatalogNavigation.IdSide)?.Description : null
                });
            }
            return cruces;
        }

        /// <summary>
        /// Se obtiene una lista de carriles
        /// </summary>
        /// <returns>Devuelve una lista de carriles</returns>
        public async Task<List<LaneCatalog>> GetLanes()
        {
            return await _dbContext.LaneCatalogs.Where(x => !string.IsNullOrWhiteSpace(x.IdLane)).AsNoTracking().ToListAsync();
        }

        public async Task<List<TypeClass>> GetClass()
        {
            return await _dbContext.TypeClasses.ToListAsync();
        }

        /// <summary>
        /// Obtiene la paginacion de todos las transacciones especificados 
        /// </summary>
        /// <param name="paginaActual">Ej. 1</param>
        /// <param name="numeroDeFilas">Ej. 10</param>
        /// <param name="tag">Ej. IMDM22961475</param>
        /// <param name="fecha">Ej. 2022-06-22 06:00:00</param>
        /// <param name="carril">Ej. B06</param>
        /// <param name="cuerpo">Ej. Cuerpo A</param>
        /// <returns>Devuelve la paginacion completa</returns>
        public async Task<List<Transaction>> GetTransactions(int? paginaActual, int? numeroDeFilas, string? tag, string? carril, string? cuerpo, DateTime? fecha, string? noDePlaca, string? noEconomico, string? clase)
        {
            IQueryable<TagList>? tagLists = _dbContext.TagLists.Where(x => x.Active);

            IQueryable<Transaction>? res = _dbContext.Transactions
                .Where(x => tagLists.Select(y => y.Tag).Contains(x.IsoContent.Trim()));

            if (!string.IsNullOrWhiteSpace(noDePlaca))
            {
                tagLists = tagLists.Where(x => x.VehiclePlate == noDePlaca);
                res = res.Where(x => tagLists.Select(y => y.Tag).Contains(x.IsoContent.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(noEconomico))
            {
                tagLists = tagLists.Where(x => x.EconomicNumber == noEconomico);
                res = res.Where(x => tagLists.Select(y => y.Tag).Contains(x.IsoContent.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(tag))
            {
                res = res.Where(x => x.IsoContent.Trim().Contains(tag.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(carril))
            {
                var lane = await _dbContext.LaneCatalogs.FirstOrDefaultAsync(x => x.IdLane.Equals(carril));
                if (lane != null) res = res.Where(x => x.IdCatalog == lane.IdCatalog);
            }
            if (!string.IsNullOrWhiteSpace(cuerpo))
            {
                var idSides = _dbContext.LaneCatalogs.Where(x => x.Description.Equals(cuerpo));
                if (idSides != null) res = res.Where(x => idSides.Select(y => y.IdSide).Contains(x.IdCatalogNavigation.IdSide));
            }
            if (fecha != null)
            {
                res = res.Where(x => x.TransactionDate.Date == fecha.Value.Date);
            }
            if (paginaActual != null && numeroDeFilas != null)
            {
                res = res.Skip((int)((paginaActual - 1) * numeroDeFilas)).Take((int)numeroDeFilas);
            }
            return await res.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Devuelve cuantas transacciones hay, filtrandolas por los parametros especificados 
        /// </summary>
        /// <param name="tag">Ej. IMDM22961475</param>
        /// <param name="fecha">Ej. 2022-06-22 06:00:00</param>
        /// <param name="carril">Ej. B06</param>
        /// <param name="cuerpo">Ej. Cuerpo A</param>
        /// <returns></returns>
        public async Task<int> GetTransactionsCount(string? tag, string? carril, string? cuerpo, DateTime? fecha, string? noDePlaca, string? noEconomico, string? clase)
        {
            IQueryable<TagList>? tagLists = _dbContext.TagLists.Where(x => x.Active);

            IQueryable<Transaction>? res = _dbContext.Transactions
                .Include(x => x.IdCatalogNavigation)
                .Where(x => tagLists.Select(y => y.Tag).Contains(x.IsoContent.Trim()));

            if (!string.IsNullOrWhiteSpace(noDePlaca))
            {
                tagLists = tagLists.Where(x => x.VehiclePlate == noDePlaca);
                res = res.Where(x => tagLists.Select(y => y.Tag).Contains(x.IsoContent.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(noEconomico))
            {
                tagLists = tagLists.Where(x => x.EconomicNumber == noEconomico);
                res = res.Where(x => tagLists.Select(y => y.Tag).Contains(x.IsoContent.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(tag))
            {
                res = res.Where(x => x.IsoContent.Trim().Contains(tag.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(carril))
            {
                var lane = await _dbContext.LaneCatalogs.FirstOrDefaultAsync(x => x.IdLane.Equals(carril));
                if (lane != null) res = res.Where(x => x.IdCatalog == lane.IdCatalog);
            }
            if (!string.IsNullOrWhiteSpace(cuerpo))
            {
                var idSides = _dbContext.LaneCatalogs.Where(x => x.Description.Equals(cuerpo));
                if (idSides != null) res = res.Where(x => idSides.Select(y => y.IdSide).Contains(x.IdCatalogNavigation.IdSide));
            }
            if (fecha != null)
            {
                res = res.Where(x => x.TransactionDate.Date == fecha.Value.Date);
            }
            if (!string.IsNullOrWhiteSpace(clase))
            {
                res = res.Where(x => x.IdClass2Navigation.ClassCode == clase);
            }

            return await res.AsNoTracking().CountAsync();
        }

        public async Task<int?[]> GetTurnos(DateTime fecha)
        {
            IQueryable<int?> res = _dbContext.Transactions.Where(x => x.TransactionDate.Date == fecha.Date).Select(x => x.IdShift).Distinct();
            return await res.ToArrayAsync();
        }
    }
}
