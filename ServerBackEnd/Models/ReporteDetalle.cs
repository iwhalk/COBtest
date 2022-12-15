namespace ApiGateway.Models
{
    public class ReporteDetalle
    {
        public int idBuilding { get; set; }
        public List<int> idApartments { get; set; }
        public List<int> idActivy { get; set; }
        public List<int> idElement { get; set; }
        public List<int>? idSubElements { get; set; }
    }
}
