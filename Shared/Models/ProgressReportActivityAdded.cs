namespace SharedLibrary.Models
{
    public class ProgressReportActivityAdded
    {
        public int IdProgressReport { get; set; }
        public DateTime DateCreated { get; set; }
        public int IdApartment { get; set; }
        public int IdArea { get; set; }
        public int? IdActivity { get; set; }
        public int IdElement { get; set; }
        public string TotalPieces { get; set; }
    }
}
