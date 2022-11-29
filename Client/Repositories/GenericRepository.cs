using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Client.Interfaces;
using SharedLibrary;
using Client.Stores;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Client.Repositories
{
    public class GenericRepository : IGenericRepository
    {
        private readonly ApplicationContext _context;
        private readonly HttpClient _httpClient;

        public GenericRepository(ApplicationContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        #region Get
        public async Task<HttpResponseMessage> GetAsync(string path)
        {
            try
            {

            }
            catch (AccessTokenNotAvailableException ex)
            {
                ex.Redirect(requestOptions =>
                {
                    requestOptions.TryAddAdditionalParameter("login_hint", "user@example.com");
                });
                throw;
            }
            using HttpResponseMessage httpResponse = await _httpClient.GetAsync(path);
            return await ParseHttpResponseAsync(httpResponse);
        }

        public async Task<T>? GetAsync<T>(string path)
        {
            try
            {
                using HttpResponseMessage httpResponse = await _httpClient.GetAsync(path);
                return await ParseHttpResponseAsync<T>(httpResponse);
            }
            catch (AccessTokenNotAvailableException ex)
            {
                ex.Redirect(requestOptions =>
                {
                    requestOptions.TryAddAdditionalParameter("login_hint", "user@example.com");
                });
                throw;
            }
        }
        #endregion

        #region Post
        public async Task<HttpResponseMessage> PostAsync(string path, object value)
        {
            try
            {
                using HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync(path, value);
                return await ParseHttpResponseAsync(httpResponse);

            }
            catch (AccessTokenNotAvailableException ex)
            {
                ex.Redirect(requestOptions =>
                {
                    requestOptions.TryAddAdditionalParameter("login_hint", "user@example.com");
                });
                throw;
            }
        }

        public async Task<T>? PostAsync<T>(string path, HttpContent value)
        {
            try
            {
                using HttpResponseMessage httpResponse = await _httpClient.PostAsync(path, value as HttpContent);
                return await ParseHttpResponseAsync<T>(httpResponse);

            }
            catch (AccessTokenNotAvailableException ex)
            {
                ex.Redirect(requestOptions =>
                {
                    requestOptions.TryAddAdditionalParameter("login_hint", "user@example.com");
                });
                throw;
            }
        }
        public async Task<T>? PostAsync<T>(string path, T value)
        {
            try
            {
                using HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync(path, value);
                return await ParseHttpResponseAsync<T>(httpResponse);

            }
            catch (AccessTokenNotAvailableException ex)
            {
                ex.Redirect(requestOptions =>
                {
                    requestOptions.TryAddAdditionalParameter("login_hint", "user@example.com");
                });
                throw;
            }
        }

        public async Task<T>? PostAsync<T>(string path, object value)
        {
            try
            {
                using HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync(path, value);
                return await ParseHttpResponseAsync<T>(httpResponse);

            }
            catch (AccessTokenNotAvailableException ex)
            {
                ex.Redirect(requestOptions =>
                {
                    requestOptions.TryAddAdditionalParameter("login_hint", "user@example.com");
                });
                throw;
            }
        }
        #endregion

        #region Put
        public async Task<T>? PutAsync<T>(string path, T value)
        {
            try
            {
                using HttpResponseMessage httpResponse = await _httpClient.PutAsJsonAsync(path, value);
                return await ParseHttpResponseAsync<T>(httpResponse);

            }
            catch (AccessTokenNotAvailableException ex)
            {
                ex.Redirect(requestOptions =>
                {
                    requestOptions.TryAddAdditionalParameter("login_hint", "user@example.com");
                });
                throw;
            }
        }

        public async Task<T>? PutAsync<T>(string path, HttpContent value)
        {
            try
            {
                using HttpResponseMessage httpResponse = await _httpClient.PutAsJsonAsync(path, value as HttpContent);
                return await ParseHttpResponseAsync<T>(httpResponse);

            }
            catch (AccessTokenNotAvailableException ex)
            {
                ex.Redirect(requestOptions =>
                {
                    requestOptions.TryAddAdditionalParameter("login_hint", "user@example.com");
                });
                throw;
            }
        }
        #endregion

        private async Task<T>? ParseHttpResponseAsync<T>(HttpResponseMessage httpResponse)
        {
            if (httpResponse.Content != null)
            {
                var mediaType = httpResponse.Content.Headers.ContentType?.MediaType;
                if (mediaType != null)
                {
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        var apiResponse = mediaType switch
                        {
                            "application/json" => await httpResponse.Content.ReadFromJsonAsync<ApiResponse>(),
                            "text/json" => await httpResponse.Content.ReadFromJsonAsync<ApiResponse>(),
                            _ => default,
                        };
                        if (apiResponse != null && apiResponse.Succeeded)
                        {
                            return new ApiResponse<T>(apiResponse).Content;
                        }
                        if (apiResponse == null || apiResponse.Succeeded)
                        {
                            var streamResponse = mediaType switch
                            {
                                string a when a.Contains("application",
                                                         StringComparison.CurrentCultureIgnoreCase) => await httpResponse.Content.ReadAsByteArrayAsync(),
                                string b when b.Contains("image",
                                                         StringComparison.CurrentCultureIgnoreCase) => await httpResponse.Content.ReadAsByteArrayAsync(),
                                _ => default,
                            };
                            return (T)Convert.ChangeType(streamResponse, typeof(T));
                        }
                        if (apiResponse != null && !apiResponse.Succeeded)
                        {
                            _context.ErrorMessage = apiResponse.Status switch
                            {
                                400 => "Bad request al servicio: " + apiResponse.ErrorMessage,
                                401 => "Lammada no autentificada al servicio.",
                                500 => "Error interno del servicio: " + apiResponse.ErrorMessage,
                                _ => "Error en el servicio",
                            };
                            return default;
                        }
                    }

                    _context.ErrorMessage = mediaType switch
                    {
                        "application/problem+json" => (await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>())?.Detail ?? "Error en el servidor",
                        "application/json" => (await httpResponse.Content.ReadFromJsonAsync<ApiResponse>())?.ErrorMessage ?? "Error en el servidor",
                        "text/plain" => await httpResponse.Content.ReadAsStringAsync(),
                        _ => "Error en el servidor",
                    };
                }
            }
            return default;
        }

        private async Task<HttpResponseMessage> ParseHttpResponseAsync(HttpResponseMessage httpResponse)
        {
            if (httpResponse.Content != null)
            {
                var mediaType = httpResponse.Content.Headers.ContentType?.MediaType;
                if (mediaType != null && !httpResponse.IsSuccessStatusCode)
                {
                    _context.ErrorMessage = mediaType switch
                    {
                        "application/problem+json" => (await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>())?.Detail ?? "Error en el servidor",
                        "text/plain" => await httpResponse.Content.ReadAsStringAsync(),
                        _ => "Error en el servidor",
                    };
                }
            }
            return httpResponse;
        }

    }

    internal class ProblemDetails
    {
        /// <summary>
        /// A URI reference [RFC3986] that identifies the problem type. This specification encourages that, when
        /// dereferenced, it provide human-readable documentation for the problem type
        /// (e.g., using HTML [W3C.REC-html5-20141028]).  When this member is not present, its value is assumed to be
        /// "about:blank".
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// A short, human-readable summary of the problem type.It SHOULD NOT change from occurrence to occurrence
        /// of the problem, except for purposes of localization(e.g., using proactive content negotiation;
        /// see[RFC7231], Section 3.4).
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>
        /// The HTTP status code([RFC7231], Section 6) generated by the origin server for this occurrence of the problem.
        /// </summary>
        [JsonPropertyName("status")]
        public int? Status { get; set; }

        /// <summary>
        /// A human-readable explanation specific to this occurrence of the problem.
        /// </summary>
        [JsonPropertyName("detail")]
        public string? Detail { get; set; }

        /// <summary>
        /// A URI reference that identifies the specific occurrence of the problem.It may or may not yield further information if dereferenced.
        /// </summary>
        [JsonPropertyName("instance")]
        public string? Instance { get; set; }

        /// <summary>
        /// Gets the <see cref="IDictionary{TKey, TValue}"/> for extension members.
        /// <para>
        /// Problem type definitions MAY extend the problem details object with additional members. Extension members appear in the same namespace as
        /// other members of a problem type.
        /// </para>
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, object>? Extensions { get; set; }
    }
}
