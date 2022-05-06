using ReportesData.Models;
using System.Collections;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;

namespace ApiGateway.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public int Status { get; set; }
        public string? ContentType { get; set; }
        public object? Content { get; set; }
        public string? ErrorMessage { get; internal set; }
    }
    public class ApiResponse<TClass>
    {
        public bool Success { get; set; }
        public int Status { get; set; }
        public string? ContentType { get; set; }
        public TClass? Content { get; set; }
        public string? ErrorMessage { get; internal set; }

        public ApiResponse()
        {
        }
        public ApiResponse(ApiResponse apiResponse)
        {
            Success = apiResponse.Success;
            Status = apiResponse.Status;
            ContentType = apiResponse.ContentType;
            ErrorMessage = apiResponse.ErrorMessage;
            Content = apiResponse.Content?.GetType() switch
            {
                var cls when cls == typeof(JsonElement) => ((JsonElement)apiResponse.Content).Deserialize<TClass>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }),
                var cls when cls == typeof(TClass) => (TClass)apiResponse.Content,
                _ => default,
            };
        }
    }
}
