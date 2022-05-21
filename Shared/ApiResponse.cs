using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared
{
    public class ApiResponse
    {
        public bool Succeeded { get; set; }
        public int Status { get; set; }
        public string? ContentType { get; set; }
        public object? Content { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ErrorMessage { get; set; }
    }
    public class ApiResponse<TClass>
    {
        public bool Succeeded { get; set; }
        public int Status { get; set; }
        public string? ContentType { get; set; }
        public TClass? Content { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ErrorMessage { get; set; }

        public ApiResponse()
        {
        }
        public ApiResponse(ApiResponse apiResponse)
        {
            Succeeded = apiResponse.Succeeded;
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
