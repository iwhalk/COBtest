using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Models;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using ReportesInmobiliaria.Interfaces;

namespace ReportesInmobiliaria.Services
{
    public class TagsService : ITagsService
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TagsService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Devuelve cuantas veces se encuntra ese tag en la tabla de TagList de la base de datos
        /// </summary>
        /// <param name="tag">Ej. IMDM22961475</param>
        /// <param name="estatus">Ej. true o false</param>
        /// <param name="fecha">Ej. 2022-06-22 06:00:00</param>
        /// <returns>Devuelve el numero de veces que esta el tag dado anteriormene en la tabla TagList</returns>
        public async Task<int> GetTagsCountAsync(string? tag, bool? estatus, DateTime? fecha, string? noDePlaca, string? noEconomico)
        {
            IQueryable<TagList>? res = _dbContext.TagLists;

            if (!string.IsNullOrWhiteSpace(tag))
            {
                res = res.Where(x => x.Tag.Trim().Contains(tag.Trim()));
            }
            if (estatus != null)
            {
                res = res.Where(x => x.Active == estatus);
            }
            if (fecha != null)
            {
                res = res.Where(x => x.InsertionDate.Date == fecha.Value.Date);
            }
            if (!string.IsNullOrWhiteSpace(noDePlaca))
            {
                res = res.Where(x => x.VehiclePlate.Equals(noDePlaca));
            }
            if (!string.IsNullOrWhiteSpace(noEconomico))
            {
                res = res.Where(x => x.EconomicNumber.Equals(noEconomico));
            }

            return await res.CountAsync();
        }

        /// <summary>
        /// Obtiene la paginacion de la lista de tags
        /// </summary>
        /// <param name="paginaActual">Ej. 1</param>
        /// <param name="numeroDeFilas">Ej. 10</param>
        /// <param name="tag">Ej. IMDM22961475</param>
        /// <param name="estatus">Ej. true o false</param>
        /// <param name="fecha">Ej. 2022-06-22 06:00:00</param>
        /// <returns></returns>
        public async Task<List<TagList>> GetTagsAsync(int? paginaActual, int? numeroDeFilas, string? tag, bool? estatus, DateTime? fecha, string? noDePlaca, string? noEconomico)
        {
            IQueryable<TagList>? res = _dbContext.TagLists.OrderByDescending(x => x.InsertionDate);

            if (!string.IsNullOrWhiteSpace(tag))
            {
                res = res.Where(x => x.Tag.Trim().Contains(tag.Trim()));
            }
            if (estatus != null)
            {
                res = res.Where(x => x.Active == estatus);
            }
            if (fecha != null)
            {
                res = res.Where(x => x.InsertionDate.Date == fecha.Value.Date);
            }
            if (!string.IsNullOrWhiteSpace(noDePlaca))
            {
                res = res.Where(x => x.VehiclePlate.Equals(noDePlaca));
            }
            if (!string.IsNullOrWhiteSpace(noEconomico))
            {
                res = res.Where(x => x.EconomicNumber.Equals(noEconomico));
            }
            if (paginaActual != null && numeroDeFilas != null && string.IsNullOrWhiteSpace(tag))
            {
                res = paginaActual > 1 ? res.Skip((int)((paginaActual - 1) * numeroDeFilas)).Take((int)numeroDeFilas) : res.Take((int)numeroDeFilas);
            }

            return await res.ToListAsync();
        }

        /// <summary>
        /// Obtiene las lista de tags via pass
        /// </summary>      
        /// <param name="tag"></param>
        /// <returns>Devuelve una colecion de ViaPassTags sin sus TagList asociados</returns>
        public async Task<List<Viapasstags>> GetViaPassTagsAsync(string? tag)
        {
            IQueryable<Viapasstags> viaPassTags = _dbContext.Viapasstags;
            var listTags = _dbContext.TagLists;

            if (!string.IsNullOrWhiteSpace(tag))
            {
                viaPassTags = viaPassTags.Where(x => x.Tag.Equals(tag));
            }

            return await viaPassTags.Where(x => listTags.Select(y => y.Tag).Contains(x.Tag) == false).ToListAsync();
        }

        /// <summary>
        /// Actualiza el tag en la base de datos
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>Devuelve un true si todo salio bien</returns>
        public async Task<bool> UpdateTagAsync(TagList tag)
        {
            _dbContext.Entry(tag).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return true;
        }

        /// <summary>
        /// Inserta un nuevo tag en la base datos
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>Devuelve el tag anteriormente ingresado</returns>
        public async Task<TagList> CreateTagAsync(TagList tag)
        {
            await _dbContext.TagLists.AddAsync(tag);
            await _dbContext.SaveChangesAsync();
            return tag;
        }

        /// <summary>
        /// Elimna el tag especificado de la base de datos
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Devuelve un true si todo salio bien</returns>
        public async Task<bool> DeleteTagAsync(string id)
        {
            TagList? tag = _dbContext.TagLists.FirstOrDefault(x => x.Tag == id);
            if (tag == null) return false;

            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("NombreCompleto") ?? "Usuario no logueado";
            _ = _dbContext.LogTagLists.Add(new() { IdUser = userId, Tag = tag.Tag, UpdatedDate = DateTime.Now });

            _dbContext.TagLists.Remove(tag);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
