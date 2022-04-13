namespace ApiGateway.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string? ContentType { get; set; }
        public object? Content { get; set; }
    }
}
