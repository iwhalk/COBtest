using Obra.Client.Interfaces;
using Obra.Client.Models;
using Obra.Client.Stores;
using SharedLibrary;
using SharedLibrary.Models;

namespace Obra.Client.Services
{
    public class BlobsService : IBlobsService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public BlobsService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }
        public async Task<Blob> GetBlobAsync(int? id)
        {
            return await _repository.GetAsync<Blob>(id, path: "api/Blobs");
        }
        public async Task<List<Blob>> GetBlobsAsync(int? id)
        {
            Dictionary<string, string> parameters = new();

            if (id != null)
            {
                parameters.Add("id", id.ToString());
            }

            return await _repository.GetAsync<List<Blob>>(path: "api/Blobs", parameters: parameters);
        }  
        public async Task<Blob> PostBlobAsync(Blob blob)
        {
            return await _repository.PostAsync(blob, path: "api/Blobs");
        }
        public async Task<Blob> PostBlobAsync(BlobFile blobFile)
        {
            if (blobFile == null || blobFile.Blob == null || blobFile.FileStream == null) return null;
            var content = SerializeMultipartFormDataContent(blobFile);

            var res = await _repository.PostAsync<Blob>(content as HttpContent, path: "api/Blobs");
            if (res != null)
            {
                return res;
            }
            return new();

        }
        private static MultipartFormDataContent SerializeMultipartFormDataContent(BlobFile blobFile)
        {
            MultipartFormDataContent content = new();
            content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data");

            //content.Add(new StringContent(blobFile.Blob?.BlobTypeId?.ToString()), "BlobTypeId");
            content.Add(new StringContent(blobFile.Blob?.ContentType ?? ""), "ContentType");

            content.Add(new StreamContent(blobFile.FileStream, Convert.ToInt32(blobFile.Blob?.BlobSize)), "file", blobFile.Blob?.BlobName ?? "");

            return content;
        }

        public async Task<byte[]> GetBlobImage(int id)
        {
            return await _repository.GetAsync(id, path: "api/Blobs/Image");
        }
    }
}
