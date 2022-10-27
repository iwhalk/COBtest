using Shared.Models;

namespace ReportesInmobiliaria.Interfaces
{
    public interface ITagsService
    {
        Task<List<TagList>> GetTagsAsync(int? paginaActual, int? numeroDeFilas, string? tag, bool? estatus, DateTime? fecha, string? noDePlaca, string? noEconomico);
        Task<int> GetTagsCountAsync(string? tag, bool? estatus, DateTime? fecha, string? noDePlaca, string? noEconomico);
        Task<bool> UpdateTagAsync(TagList tag);
        Task<TagList> CreateTagAsync(TagList tag);
        Task<bool> DeleteTagAsync(string tag);
        Task<List<Viapasstags>> GetViaPassTagsAsync(string? tag);
    }
}
