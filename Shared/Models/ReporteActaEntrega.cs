namespace Shared.Models
{
    public class ReporteActaEntrega
    {
        public List<SP_GET_AERI_HEADERResult> header { get; set; }
        public List<SP_GET_AERI_AREASResult> areas { get; set; }
        public List<SP_GET_AERI_DELIVERABLESResult> deliverables { get; set; }
    }
}
