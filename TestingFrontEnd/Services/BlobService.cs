using FrontEnd.Interfaces;
using FrontEnd.Models;
using FrontEnd.Stores;
using SharedLibrary.Models;

namespace FrontEnd.Services
{
    public class BlobService : IBlobService
    {
        private readonly IGenericRepository _repository;
        private readonly ApplicationContext _context;
        public BlobService(IGenericRepository repository, ApplicationContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<Blob>> GetBlobsAsync()
        {
            if (_context.Blobs == null)
            {
                var response = await _repository.GetAsync<List<Blob>>("api/Blobs");

                if (response != null)
                {
                    _context.Blobs = response;
                    return _context.Blobs;
                }
            }

            return _context.Blobs;
        }

        public async Task<Blob> PostBlobAsync(Blob blob)
        {
            return await _repository.PostAsync("api/Blobs", blob);
        }
        public async Task<Blob> PostBlobAsync(BlobFile blobFile)
        {
            if (blobFile == null || blobFile.Blob == null || blobFile.FileStream == null) return null;
            var content = SerializeMultipartFormDataContent(blobFile);

            var res = await _repository.PostAsync<Blob>("api/Blobs", content as HttpContent);
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

            content.Add(new StringContent(blobFile.Blob?.BlodTypeId.ToString()), "BlobTypeId");
            content.Add(new StringContent(blobFile.Blob?.ContentType ?? ""), "ContentType");

            content.Add(new StreamContent(blobFile.FileStream, Convert.ToInt32(blobFile.Blob?.BlobSize)), "file", blobFile.Blob?.BlodName ?? "");

            return content;
        }
    }
}
