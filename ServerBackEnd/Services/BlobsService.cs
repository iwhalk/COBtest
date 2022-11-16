using ApiGateway.Interfaces;
using ApiGateway.Proxies;
using FrontEnd.Models;
using SharedLibrary;
using SharedLibrary.Models;

namespace ApiGateway.Services
{
    public class BlobsService : GenericProxy, IBlobsService
    {
        public BlobsService(IHttpContextAccessor? httpContextAccessor, IHttpClientFactory httpClientFactory) : base(httpContextAccessor, httpClientFactory, "Reportes")
        {

        }

        public async Task<ApiResponse<bool>> DeleteBlobAsync(int idBlob)
        {
            return await DeleteAsync<bool>(idBlob, path: "Blob");
        }

        public async Task<ApiResponse<List<Blob>>> GetBlobsAsync()
        {
            return await GetAsync<List<Blob>>(path: "Blobs");
        }

        public async Task<ApiResponse<Blob>> PostBlobAsync(Blob blob, IFormFile file)
        {
            var content = SerializeMultipartFormDataContent(new() { FileStream = file.OpenReadStream(), Blob = blob });
            return await PostAsync<Blob>(content as HttpContent, path: "Blobs?name=test");
        }

        public async Task<ApiResponse<Blob>> PutBlobAsync(Blob blob)
        {
            return await PutAsync<Blob>(blob.IdBlobs, blob, path: "Blobs");
        }
        private static MultipartFormDataContent SerializeMultipartFormDataContent(BlobFile blobFile)
        {
            MultipartFormDataContent content = new();
            content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data");

            content.Add(new StringContent(blobFile.Blob?.BlodTypeId.ToString()), "BlobTypeId");
            content.Add(new StringContent(blobFile.Blob?.ContentType ?? ""), "ContentType");

            content.Add(new StreamContent(blobFile.FileStream, Convert.ToInt32(blobFile.Blob?.BlobSize)), name: "file", fileName: "file");

            return content;
        }
    }
}
