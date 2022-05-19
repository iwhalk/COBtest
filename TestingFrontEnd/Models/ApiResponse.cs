namespace TestingFrontEnd.Models
{
    public class ApiResponse
    {
        public bool Succeeded { get; set; }
        public int Status { get; set; }
        public string? ContentType { get; set; }
        public object? Content { get; set; }
        public string? ErrorMessage { get; internal set; }
    }
    public class ApiResponse<TClass>
    {
        public bool Succeeded { get; set; }
        public int Status { get; set; }
        public string? ContentType { get; set; }
        public TClass? Content { get; set; }
        public string? ErrorMessage { get; internal set; }
    }
}
