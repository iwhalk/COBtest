using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Client.Interfaces;
using SharedLibrary;
using Client.Stores;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using PluralizeService.Core;

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
                using HttpResponseMessage httpResponse = await _httpClient.GetAsync(path);
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

        /// <summary>
        /// Realiza una llamada http GET de un solo objeto con ID al endpoint del servicio especificado
        /// </summary>
        /// <param name="id">Identificador primario del objeto solicitado</param>
        /// <param name="parameters">Diccionario de parametros a embebir en la URI en formato "llave","valor"</param>
        /// <param name="path">Ruta del servicio, si no se especifica se asumira el nombre del objeto como ruta</param>
        /// <returns>Respuesta en un objeto generico del endpoint del servicio consultado serializada en formato JSON</returns>

        public async Task<T> GetAsync<T>(object? id, Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.GetAsync(GetUri<T>(id, parameters, path));
            return await ParseHttpResponseAsync<T>(httpResponse);
        }
        public async Task<object> GetAsync(object? id, Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.GetAsync(GetUri<object>(id, parameters, path));
            return await ParseHttpResponseAsync(httpResponse);
        }

        /// <summary>
        /// Realiza una llamada http GET al endpoint del servicio especificado
        /// </summary>
        /// <param name="parameters">Diccionario de parametros a embebir en la URI en formato "llave","valor"</param>
        /// <param name="path">Ruta del servicio, si no se especifica se asumira el nombre del objeto como ruta</param>
        /// <returns>Respuesta en un arreglo de objetos genericos del endpoint del servicio consultado serializada en formato JSON</returns>
        public async Task<T> GetAsync<T>(Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.GetAsync(GetUri<T>(default, parameters, path));
            return await ParseHttpResponseAsync<T>(httpResponse);
        }
        public async Task<object> GetAsync(Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.GetAsync(GetUri<object>(default, parameters, path));
            return await ParseHttpResponseAsync(httpResponse);
        }
        #endregion

        /// <summary>
        /// Realiza una llamada http POST al endpoint del servicio especificado
        /// </summary>
        /// <param name="value">Objeto poblado del tipo definido en la clase</param>
        /// <param name="parameters">Diccionario de parametros a embebir en la URI en formato "llave","valor"</param>
        /// <param name="path">Ruta del servicio, si no se especifica se asumira el nombre del objeto como ruta</param>
        /// <returns>Respuesta en un objeto del tipo definido en la clase del endpoint del servicio consultado</returns>
        public async Task<T> PostAsync<T>(T? value, Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync(GetUri<T>(default, parameters, path), value);
            return await ParseHttpResponseAsync<T>(httpResponse);
        }
        public async Task<T> PostAsync<T>(object? value, Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync(GetUri<object>(default, parameters, path), value);
            return await ParseHttpResponseAsync<T>(httpResponse);
        }
        public async Task<object> PostAsync(object? value, Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync(GetUri<object>(default, parameters, path), value);
            return await ParseHttpResponseAsync(httpResponse);
        }

        /// <summary>
        /// Realiza una llamada http POST al endpoint del servicio especificado
        /// </summary>
        /// <param name="content">Objeto serializado en datos por multiples partes</param>
        /// <param name="parameters">Diccionario de parametros a embebir en la URI en formato "llave","valor"</param>
        /// <param name="path">Ruta del servicio, si no se especifica se asumira el nombre del objeto como ruta</param>
        /// <returns>Respuesta en un objeto del tipo definido en la clase del endpoint del servicio consultado</returns>
        public async Task<T> PostAsync<T>(HttpContent? content, Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.PostAsync(GetUri<T>(default, parameters, path), content);
            return await ParseHttpResponseAsync<T>(httpResponse);
        }
        public async Task<object> PostAsync(HttpContent? content, Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.PostAsync(GetUri<object>(default, parameters, path), content);
            return await ParseHttpResponseAsync(httpResponse);
        }

        /// <summary>
        /// Realiza una llamada http PUT al endpoint del servicio especificado
        /// </summary>
        /// <param name="id">Identificador primario del objeto a modificar</param>
        /// <param name="value">Objeto poblado del tipo definido en la clase</param>
        /// <param name="parameters">Diccionario de parametros a embebir en la URI en formato "llave","valor"</param>
        /// <param name="path">Ruta del servicio, si no se especifica se asumira el nombre del objeto como ruta</param>
        /// <returns>Respuesta de tipo bool si la operacion se completo de manera satisfactoria en el endpoint del servicio consultado</returns>
        public async Task<T> PutAsync<T>(object? id, T? value, Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.PutAsJsonAsync(GetUri<T>(id, parameters, path), value);
            return await ParseHttpResponseAsync<T>(httpResponse);
        }
        public async Task<object> PutAsync(object? id, object? value, Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.PutAsJsonAsync(GetUri<object>(id, parameters, path), value);
            return await ParseHttpResponseAsync(httpResponse);
        }

        /// <summary>
        /// Realiza una llamada http PUT al endpoint del servicio especificado
        /// </summary>
        /// <param name="id">Identificador primario del objeto a modificar</param>
        /// <param name="content">Objeto serializado en datos por multiples partes</param>
        /// <param name="parameters">Diccionario de parametros a embebir en la URI en formato "llave","valor"</param>
        /// <param name="path">Ruta del servicio, si no se especifica se asumira el nombre del objeto como ruta</param>
        /// <returns>Respuesta de tipo bool si la operacion se completo de manera satisfactoria en el endpoint del servicio consultado</returns>
        public async Task<T> PutAsync<T>(object? id, HttpContent? content, Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.PutAsync(GetUri<T>(id, parameters, path), content);
            return await ParseHttpResponseAsync<T>(httpResponse);
        }
        public async Task<object> PutAsync(object? id, HttpContent? content, Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.PutAsync(GetUri<object>(id, parameters, path), content);
            return await ParseHttpResponseAsync(httpResponse);
        }
        /// <summary>
        /// Realiza una llamada http DELETE al endpoint del servicio especificado
        /// </summary>
        /// <param name="id">Identificador primario del objeto a modificar</param>
        /// <param name="parameters">Diccionario de parametros a embebir en la URI en formato "llave","valor"</param>
        /// <param name="path">Ruta del servicio, si no se especifica se asumira el nombre del objeto como ruta</param>
        /// <returns>Respuesta de tipo bool si la operacion se completo de manera satisfactoria en el endpoint del servicio consultado</returns>
        public async Task<T> DeleteAsync<T>(object? id, Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.DeleteAsync(GetUri<T>(id, parameters, path));
            return await ParseHttpResponseAsync<T>(httpResponse);
        }
        //public async Task<ApiResponse> DeleteAsync(object? id, Dictionary<string, string>? parameters = null, string? path = null)
        //{
        //    using HttpResponseMessage httpResponse = await _httpClient.DeleteAsync(GetUri<object>(id, parameters, path));
        //    return await ParseHttpResponseAsync(httpResponse);
        //}

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

        /// <summary>
        /// Obtiene la URI codificada del endpoint del servicio a consultar
        /// </summary>
        /// <param name="id">Identificador primario del objeto</param>
        /// <param name="parameters">Diccionario de parametros a embebir en la URI en formato "llave","valor"</param>
        /// <param name="path">Ruta del servicio, si no se especifica se asumira el nombre del objeto como ruta</param>
        /// <returns>URI codificada en una cadena</returns>
        private static string? GetUri<T>(object? id, Dictionary<string, string>? parameters = null, string? path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                if (typeof(T) != typeof(object))
                {
                    string typeName = PluralizationProvider.Pluralize(typeof(T).Name);
                    string controllerName = new(typeName.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray());
                    path = $"api/{controllerName}";
                }
            }

            if (id != null && !id.Equals(0))
            {
                path += $"/{WebUtility.HtmlEncode(id.ToString())}";
            }

            return parameters == null ? path : QueryHelpers.AddQueryString(path, parameters);
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
