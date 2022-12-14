namespace ApiGateway.Models
{
    public class ReporteDetalle
    {
        public int idBuilding { get; set; }
        public int[] idApartments { get; set; }
        public int[] idActivities { get; set; }
        public int[] idElements { get; set; }
        public int[]? idSubElements { get; set; }
    }
}
