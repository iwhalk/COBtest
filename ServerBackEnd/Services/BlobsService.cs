using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using ApiGateway.Models;
using SharedLibrary;
using SharedLibrary.Models;
using Obra.Client.Models;

namespace ApiGateway.Services
{
    public class BlobsService : GenericProxy, IBlobsService
    {
        public BlobsService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<byte[]>> GetBlobImage(int id)
        {
            return await GetAsync<byte[]>(id, path: "BlobImage");
        }

        public async Task<ApiResponse<Blob>> GetBlobAsync(int id)
        {
            return await GetAsync<Blob>(id, path: "Blob");
        }

        public async Task<ApiResponse<List<Blob>>> GetBlobsAsync(int? id)
        {
            Dictionary<string, string> parameters = new();

            if (id != null && id > 0)
            {
                parameters.Add("id", id.ToString());
            }

            return await GetAsync<List<Blob>>(path: "Blobs", parameters: parameters);
        }

        public async Task<ApiResponse<Blob>> PostBlobAsync(Blob blob, IFormFile file)
        {
            var content = SerializeMultipartFormDataContent(new() { FileStream = file.OpenReadStream(), Blob = blob });
            return await PostAsync<Blob>(content as HttpContent, path: "Blob");
        }

        private static MultipartFormDataContent SerializeMultipartFormDataContent(BlobFile blobFile)
        {
            MultipartFormDataContent content = new();
            content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data");

            content.Add(new StringContent(blobFile.Blob?.BlobTypeId.ToString()), "BlobTypeId");
            content.Add(new StringContent(blobFile.Blob?.ContentType ?? ""), "ContentType");

            content.Add(new StreamContent(blobFile.FileStream, Convert.ToInt32(blobFile.Blob?.BlobSize)), name: "file", fileName: "file");

            return content;
        }
    }
}
