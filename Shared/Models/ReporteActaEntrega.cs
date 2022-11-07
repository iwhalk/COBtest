namespace Shared.Models
{
    public class ReporteActaEntrega
    {
        public DateTime generationDate { get; set; }
        public string lessor { get; set; }
        public string tenant { get; set; }
        public Property? property { get; set; }
        public ReceptionCertificate numeroDeContrato { get; set; }
        public List<InventoryToReports?> inventories { get; set; }
        public AspNetUser user { get; set; }
    }
}
