using ApiGateway.Extensions;
using ApiGateway.Models;
using Microsoft.AspNetCore.WebUtilities;
using PluralizeService.Core;
using System.Net;
using System.Text;

namespace ApiGateway.Proxies
{
    public abstract class GenericProxy<TClass> where TClass : class
    {
        private readonly HttpClient _httpClient;

        protected GenericProxy(IHttpContextAccessor httpContextAccessor, HttpClient httpClient, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("WeatherForecast");

            _httpClient.AddBearerToken(httpContextAccessor);
        }

        //private async Task<HttpClient> GetHttpClientAsync()
        //{
        //}

        public async Task<TClass?> GetAsync(object id, Dictionary<string, string>? parameters = null, string? path = null)
        {
            try
            {
                //_httpClient = await GetHttpClientAsync();
                using HttpResponseMessage httpResponse = await _httpClient.GetAsync(GetUri(id, parameters, path));
                if (httpResponse.IsSuccessStatusCode)
                {
                    return await httpResponse.Content.ReadFromJsonAsync<TClass>();
                }
                else
                {
                    Console.WriteLine(await httpResponse.Content?.ReadAsStringAsync());
                    return null;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ApiResponse> GetAsync(Dictionary<string, string>? parameters = null, string? path = null)
        {
            try
            {
                using HttpResponseMessage httpResponse = await _httpClient.GetAsync(GetUri(default, parameters, path));
                return await ParseHttpResponseAsync(httpResponse);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new()
                {
                    Success = false,
                    StatusCode = 500,
                    Content = e.Message,
                };
            }
        }
        public async Task<byte[]> GetStreamAsync(Dictionary<string, string>? parameters = null, string? path = null)
        {
            try
            {
                using HttpResponseMessage httpResponse = await _httpClient.GetAsync(GetUri(default, parameters, path));

                return await httpResponse.Content.ReadAsByteArrayAsync();
                //return await ParseHttpResponseAsync(httpResponse);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
                //return new()
                //{
                //    Success = false,
                //    StatusCode = 500,
                //    Content = e.Message,
                //};
            }
        }

        public async Task<TClass> PostAsync(TClass data, Dictionary<string, string> parameters = null, string path = null)
        {
            try
            {
                //_httpClient = await GetHttpClientAsync();
                using HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync(GetUri(default, parameters, path), data);
                if (httpResponse.IsSuccessStatusCode)
                {
                    return await httpResponse.Content.ReadFromJsonAsync<TClass>();
                }
                else
                {
                    Console.WriteLine(await httpResponse.Content?.ReadAsStringAsync());
                    return null;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<TClass> PostAsync(HttpContent content, Dictionary<string, string> parameters = null, string path = null)
        {
            try
            {
                //_httpClient = await GetHttpClientAsync();
                using HttpResponseMessage httpResponse = await _httpClient.PostAsync(GetUri(default, parameters, path), content);
                if (httpResponse.IsSuccessStatusCode)
                {
                    return await httpResponse.Content.ReadFromJsonAsync<TClass>();
                }
                else
                {
                    Console.WriteLine(await httpResponse.Content?.ReadAsStringAsync());
                    return null;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> PutAsync(object id, TClass data, Dictionary<string, string> parameters = null, string path = null)
        {
            try
            {
                //_httpClient = await GetHttpClientAsync();
                using HttpResponseMessage httpResponse = await _httpClient.PutAsJsonAsync(GetUri(id, parameters, path), data);
                if (httpResponse.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine(await httpResponse.Content?.ReadAsStringAsync());
                    return false;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> PutAsync(object id, HttpContent content, Dictionary<string, string> parameters = null, string path = null)
        {
            try
            {
                //_httpClient = await GetHttpClientAsync();
                using HttpResponseMessage httpResponse = await _httpClient.PutAsync(GetUri(id, parameters, path), content);
                if (httpResponse.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine(await httpResponse.Content?.ReadAsStringAsync());
                    return false;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(object id, Dictionary<string, string> parameters = null, string path = null)
        {
            try
            {
                //_httpClient = await GetHttpClientAsync();
                using HttpResponseMessage httpResponse = await _httpClient.DeleteAsync(GetUri(id, parameters, path));
                if (httpResponse.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine(await httpResponse.Content?.ReadAsStringAsync());
                    return false;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                throw;
            }
        }

        private static string GetUri(object id, Dictionary<string, string>? parameters = null, string? path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                string typeName = PluralizationProvider.Pluralize(typeof(TClass).Name);
                string controllerName = new(typeName.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray());

                path = $"api/{controllerName}";
            }

            if (id != null && !id.Equals(0))
            {
                path += $"/{WebUtility.HtmlEncode(id.ToString())}";
            }

            return parameters == null ? path : QueryHelpers.AddQueryString(path, parameters);
        }
        private static async Task<ApiResponse> ParseHttpResponseAsync(HttpResponseMessage httpResponse)
        {
            try
            {
                ApiResponse response = new()
                {
                    Success = httpResponse.IsSuccessStatusCode,
                    StatusCode = (int)httpResponse.StatusCode
                };

                if (httpResponse.Content != null)
                {
                    var mediaType = httpResponse.Content.Headers.ContentType?.MediaType;
                    if (mediaType != null)
                    {
                        response.ContentType = mediaType;
                        response.Content = mediaType switch
                        {
                            "application/json" => await httpResponse.Content.ReadFromJsonAsync<object>(),
                            "text/plain" => await httpResponse.Content.ReadAsStringAsync(),
                            "application/pdf" => await httpResponse.Content.ReadAsStreamAsync(),
                            _ => new(),
                        };
                    }
                }

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new()
                {
                    Success = false,
                    StatusCode = (int)httpResponse.StatusCode,
                    Content = e.Message,
                };
            }
        }
        //public async Task<string> GetLoggedInUserAsync()
        //{
        //    var currentAuthenticationState = await _authenticationState.GetAuthenticationStateAsync();
        //    if (!currentAuthenticationState.User.Identity.IsAuthenticated)
        //        return null;

        //    return currentAuthenticationState.User.GetLoggedInUserId<string>();
        //}
    }

}
