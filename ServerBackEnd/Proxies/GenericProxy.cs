using ApiGateway.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using PluralizeService.Core;
using Shared;
using System.Net;

namespace ApiGateway.Proxies
{
    /// <summary>
    /// Clase con los metodos para realizar llamadas http a los endpoints del servicio
    /// </summary>
    /// <typeparam name="TClass">Tipo del objeto que recibe y retorna los endpoints del servicio</typeparam>
    public abstract class GenericProxy
    {
        private readonly HttpClient _httpClient;
        //private readonly ILogger<GenericProxy> _logger;

        protected GenericProxy(IHttpContextAccessor? httpContextAccessor,
                               IHttpClientFactory httpClientFactory,
                               //ILogger<GenericProxy> logger,
                               string? httpClientName)
        {
            _httpClient = httpClientFactory.CreateClient(httpClientName);
            _httpClient.AddBearerToken(httpContextAccessor);
            //_logger = logger;
        }

        //private async Task<HttpClient> GetHttpClientAsync()
        //{
        //}

        /// <summary>
        /// Realiza una llamada http GET de un solo objeto con ID al endpoint del servicio especificado
        /// </summary>
        /// <param name="id">Identificador primario del objeto solicitado</param>
        /// <param name="parameters">Diccionario de parametros a embebir en la URI en formato "llave","valor"</param>
        /// <param name="path">Ruta del servicio, si no se especifica se asumira el nombre del objeto como ruta</param>
        /// <returns>Respuesta en un objeto generico del endpoint del servicio consultado serializada en formato JSON</returns>

        public async Task<ApiResponse<T>> GetAsync<T>(object? id, Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.GetAsync(GetUri<T>(id, parameters, path));
            return await ParseHttpResponseAsync<T>(httpResponse);
        }
        public async Task<ApiResponse> GetAsync(object? id, Dictionary<string, string>? parameters = null, string? path = null)
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
        public async Task<ApiResponse<T>> GetAsync<T>(Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.GetAsync(GetUri<T>(default, parameters, path));
            return await ParseHttpResponseAsync<T>(httpResponse);
        }
        public async Task<ApiResponse> GetAsync(Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.GetAsync(GetUri<object>(default, parameters, path));
            return await ParseHttpResponseAsync(httpResponse);
        }

        /// <summary>
        /// Realiza una llamada http POST al endpoint del servicio especificado
        /// </summary>
        /// <param name="value">Objeto poblado del tipo definido en la clase</param>
        /// <param name="parameters">Diccionario de parametros a embebir en la URI en formato "llave","valor"</param>
        /// <param name="path">Ruta del servicio, si no se especifica se asumira el nombre del objeto como ruta</param>
        /// <returns>Respuesta en un objeto del tipo definido en la clase del endpoint del servicio consultado</returns>
        public async Task<ApiResponse<T>> PostAsync<T>(T? value, Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync(GetUri<T>(default, parameters, path), value);
            return await ParseHttpResponseAsync<T>(httpResponse);
        }
        public async Task<ApiResponse<T>> PostAsync<T>(object? value, Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync(GetUri<object>(default, parameters, path), value);
            return await ParseHttpResponseAsync<T>(httpResponse);
        }
        public async Task<ApiResponse> PostAsync(object? value, Dictionary<string, string>? parameters = null, string? path = null)
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
        public async Task<ApiResponse<T>> PostAsync<T>(HttpContent? content, Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.PostAsync(GetUri<T>(default, parameters, path), content);
            return await ParseHttpResponseAsync<T>(httpResponse);
        }
        public async Task<ApiResponse> PostAsync(HttpContent? content, Dictionary<string, string>? parameters = null, string? path = null)
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
        public async Task<ApiResponse<T>> PutAsync<T>(object? id, T? value, Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.PutAsJsonAsync(GetUri<T>(id, parameters, path), value);
            return await ParseHttpResponseAsync<T>(httpResponse);
        }
        public async Task<ApiResponse> PutAsync(object? id, object? value, Dictionary<string, string>? parameters = null, string? path = null)
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
        public async Task<ApiResponse<T>> PutAsync<T>(object? id, HttpContent? content, Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.PutAsync(GetUri<T>(id, parameters, path), content);
            return await ParseHttpResponseAsync<T>(httpResponse);
        }
        public async Task<ApiResponse> PutAsync(object? id, HttpContent? content, Dictionary<string, string>? parameters = null, string? path = null)
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
        public async Task<ApiResponse<T>> DeleteAsync<T>(object? id, Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.DeleteAsync(GetUri<T>(id, parameters, path));
            return await ParseHttpResponseAsync<T>(httpResponse);
        }
        public async Task<ApiResponse> DeleteAsync(object? id, Dictionary<string, string>? parameters = null, string? path = null)
        {
            using HttpResponseMessage httpResponse = await _httpClient.DeleteAsync(GetUri<object>(id, parameters, path));
            return await ParseHttpResponseAsync(httpResponse);
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

        /// <summary>
        /// Deserializa la respuesta del endpoint del servicio en un objeto nuevo
        /// </summary>
        /// <param name="httpResponse">Respuesta de la llamada HTTP al endpoint del servicio</param>
        /// <returns>Respuesta del endpoint del servicio en un objeto nuevo</returns>
        private static async Task<ApiResponse<T>> ParseHttpResponseAsync<T>(HttpResponseMessage httpResponse)
        {
            ApiResponse<T> response = new()
            {
                Succeeded = httpResponse.IsSuccessStatusCode,
                Status = (int)httpResponse.StatusCode
            };

            if (httpResponse.Content != null)
            {
                var mediaType = httpResponse.Content.Headers.ContentType?.MediaType;
                if (mediaType != null && response.Succeeded)
                {
                    response.ContentType = mediaType;
                    response.Content = mediaType switch
                    {
                        "application/json" => await httpResponse.Content.ReadFromJsonAsync<T>(),
                        "text/json" => await httpResponse.Content.ReadFromJsonAsync<T>(),
                        string a when a.Contains("application",
                                                 StringComparison.CurrentCultureIgnoreCase) => (T)Convert.ChangeType(await httpResponse.Content.ReadAsByteArrayAsync(), typeof(T)),
                        string b when b.Contains("image",
                                                 StringComparison.CurrentCultureIgnoreCase) => (T)Convert.ChangeType(await httpResponse.Content.ReadAsByteArrayAsync(), typeof(T)),
                        _ => default,                        
                    };
                }
                if (!response.Succeeded)
                {
                    response.ErrorMessage = mediaType switch
                    {
                        "application/problem+json" => (await httpResponse.Content.ReadFromJsonAsync<HttpValidationProblemDetails>())?.Detail,
                        "text/plain" => await httpResponse.Content.ReadAsStringAsync(),
                        _ => default,
                    };
                }
            }

            return response;
        }

        private static async Task<ApiResponse> ParseHttpResponseAsync(HttpResponseMessage httpResponse)
        {
            ApiResponse response = new()
            {
                Succeeded = httpResponse.IsSuccessStatusCode,
                Status = (int)httpResponse.StatusCode
            };

            if (httpResponse.Content != null)
            {
                var mediaType = httpResponse.Content.Headers.ContentType?.MediaType;
                if (mediaType != null && response.Succeeded)
                {
                    response.ContentType = mediaType;
                    response.Content = mediaType switch
                    {
                        "application/json" => await httpResponse.Content.ReadFromJsonAsync<object>(),
                        "text/json" => await httpResponse.Content.ReadFromJsonAsync<object>(),
                        "text/plain" => await httpResponse.Content.ReadAsStringAsync(),
                        string a when a.Contains("application",
                                                 StringComparison.CurrentCultureIgnoreCase) => await httpResponse.Content.ReadAsByteArrayAsync(),
                        string b when b.Contains("image",
                                                 StringComparison.CurrentCultureIgnoreCase) => await httpResponse.Content.ReadAsByteArrayAsync(),
                        _ => new(),
                    };
                }
                if (!response.Succeeded)
                {
                    response.ErrorMessage = mediaType switch
                    {
                        "application/problem+json" => (await httpResponse.Content.ReadFromJsonAsync<HttpValidationProblemDetails>())?.Detail,
                        "text/plain" => await httpResponse.Content.ReadAsStringAsync(),
                        _ => default,
                    };
                }
            }

            return response;
        }

        private static async Task<T> GetTContentAsync<T>(HttpContent content)
        {
            if (typeof(T) == typeof(byte[]))
            {
                return (T)Convert.ChangeType(await content.ReadAsByteArrayAsync(), typeof(T));
            }
            return (T)Convert.ChangeType(await content.ReadAsStreamAsync(), typeof(T));
        }

        /// <summary>
        /// Serializa una excepción en una nueva respuesta de API
        /// </summary>
        /// <param name="e">L</param>
        /// <returns>Excepción serializada en un objeto nuevo</returns>
        //private ApiResponse SerializeException(Exception e)
        //{
        //    _logger.LogError(e, e.Message);
        //    return new()
        //    {
        //        Success = false,
        //        StatusCode = 500,
        //        Content = e.StackTrace,
        //        ErrorMessage = e.Message,
        //    };
        //}

        //public async Task<string> GetLoggedInUserAsync()
        //{
        //    var currentAuthenticationState = await _authenticationState.GetAuthenticationStateAsync();
        //    if (!currentAuthenticationState.User.Identity.IsAuthenticated)
        //        return null;

        //    return currentAuthenticationState.User.GetLoggedInUserId<string>();
        //}
    }

}
