namespace SharedLibrary.Models
{
    public class ObjectAccessUser
    {
        public List<Apartment> Apartments { get; set; }
        public List<Area> Areas { get; set; }
        public List<Activity> Activities { get; set; }
        public List<Element> Elements { get; set; }
        public List<SubElement?> SubElements { get; set; }
    }
}