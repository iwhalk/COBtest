using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore;
using ReportesObra.Interfaces;
using ReportesObra.Utilities;
using SharedLibrary.Data;
using SharedLibrary.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.IO;

namespace ReportesObra.Services
{
    public class BlobService : IBlobService
    {
        private readonly ObraDbContext _dbContext;
        private readonly BlobServiceClient _blobServiceClient;        

        public BlobService(ObraDbContext dbContext, BlobServiceClient blobServiceClient)
        {
            _dbContext = dbContext;
            _blobServiceClient = blobServiceClient;            
        }

        public async Task<BlobDownloadInfo>? GetBlobFileAsync(int id)
        {
            var blob = await _dbContext.Blobs.FindAsync(id);
            if (blob == null) return null;

            var blobContainerClient = _blobServiceClient.GetBlobContainerClient("imagescob");
            var blobClient = blobContainerClient.GetBlobClient(blob.BlobName);

            var blobFile = await blobClient.DownloadAsync();
            return blobFile;
        }

        public async Task<Blob> GetBlobAsync(int id)
        {
            return await _dbContext.Blobs.FirstOrDefaultAsync(x => x.IdBlob == id);
        }

        public async Task<List<Blob>> GetBlobsAsync(int? id)
        {
            IQueryable<Blob> blobs = _dbContext.Blobs;
            if (id != null)
                blobs = blobs.Where(x => x.IdBlob == id);
            return await blobs.ToListAsync();
        }
        public async Task<Blob?> CreateBlobAsync(IFormFile file)
        {

            var blobContainerClient = _blobServiceClient.GetBlobContainerClient("imagescob");
            var extensionFile = file.ContentType == "" ? "jpeg" : file.ContentType.Split("/")[1] ?? "jpeg";
            var blobName = Guid.NewGuid().ToString() + "." + extensionFile;
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            var newBlob = new Blob
            {
                BlobName = blobName,
                Uri = blobClient.Uri.ToString(),
                BlobSize = file.Length.ToString(),
                ContainerName = "imagescob",
                IsPrivate = false,
                BlobTypeId = "",
                ContentType = file.ContentType == "" ? "image/jpeg" : file.ContentType ?? "image/jpeg",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };

            using (var memoryStream = new MemoryStream())
            using (var image = Image.Load(file.OpenReadStream()))
            {
                var encoder = new JpegEncoder()
                {
                    Quality = file.Length > 100000 ? 15 : 30 //Use variable to set between 5-30 based on your requirements
                };
                image.Save(memoryStream, encoder);
                memoryStream.Position = 0;

                var auxiliaryMethods = new AuxiliaryMethods();
                var newImageStream = new MemoryStream();
                auxiliaryMethods.DateImage(memoryStream).Save(newImageStream, encoder);
                newImageStream.Position = 0;

                var response = await blobClient.UploadAsync(
                newImageStream,
                new BlobHttpHeaders
                {
                    ContentType = file.ContentType == "" ? "image/jpeg" : file.ContentType ?? "image/jpeg"
                });
            }


            try
            {
                await _dbContext.Blobs.AddAsync(newBlob);
                await _dbContext.SaveChangesAsync();

                return newBlob;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task<bool> UpdateBlobAsync(Blob blob)
        {
            _dbContext.Entry(blob).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return true;
        }

        public async Task<bool> DeleteBlobAsync(int id)
        {
            Blob? blob = _dbContext.Blobs.FirstOrDefault(x => x.IdBlob == id);
            if (blob == null)
                return false;

            var blobName = blob.BlobName;
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient("imagescob");
            var blobClient = blobContainerClient.GetBlobClient(blobName);
            var response = await blobClient.DeleteAsync();

            _dbContext.Blobs.Remove(blob);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}