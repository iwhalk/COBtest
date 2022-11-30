using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore;
using ReportesObra.Interfaces;
using SharedLibrary.Data;
using SharedLibrary.Models;
using System.IO;

namespace ReportesObra.Services
{
    public class BlobService //: IBlobService
    {
        private readonly ObraDbContext _dbContext;
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(ObraDbContext dbContext, BlobServiceClient blobServiceClient)
        {
            _dbContext = dbContext;
            _blobServiceClient = blobServiceClient;
        }

        public async Task<BlobDownloadInfo>? GetBlobAsync(int id)
        {
            var blob = await _dbContext.Blobs.FindAsync(id);
            if (blob == null) return null;

            var blobContainerClient = _blobServiceClient.GetBlobContainerClient("inventoryblobs");
            var blobClient = blobContainerClient.GetBlobClient(blob.BlodName);

            var blobFile = await blobClient.DownloadAsync();
            return blobFile;
        }
        public async Task<Blob?> CreateBlobAsync(string name, IFormFile file)
        {

            try
            {
                var blobContainerClient = _blobServiceClient.GetBlobContainerClient("inventoryblobs");
                var extensionFile = file.ContentType == null ? "jpg" : file.ContentType.Split("/")[1];
                var blobName = Guid.NewGuid().ToString() + "." + extensionFile;
                var blobClient = blobContainerClient.GetBlobClient(blobName);

                var newBlob = new Blob
                {
                    BlodName = blobName,
                    Uri = blobClient.Uri.ToString(),
                    BlobSize = file.Length.ToString(),
                    ContainerName = "inventoryblobs",
                    IsPrivate = false,
                    BlodTypeId = "",
                    ContentType = file.ContentType ?? "image/jpg",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now
                };

                var response = await blobClient.UploadAsync(
                file.OpenReadStream(),
                new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                });

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
            Blob? blob = _dbContext.Blobs.FirstOrDefault(x => x.IdBlobs == id);
            if (blob == null)
                return false;
            _dbContext.Blobs.Remove(blob);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        //public async Task<List<BlobsInventory>?> GetBlobInventoryAsync()
        //{
        //    return await _dbContext.BlobsInventories.ToListAsync();
        //}

        //public async Task<BlobsInventory?> CreateBlobInventoryAsync(BlobsInventory blobsInventory)
        //{
        //    await _dbContext.BlobsInventories.AddAsync(blobsInventory);
        //    try
        //    {
        //        await _dbContext.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        throw;
        //    }
        //    return blobsInventory;
        //}

        //public async Task<bool> UpdateBlobInventoryAsync(BlobsInventory blobsInventory)
        //{
        //    _dbContext.Entry(blobsInventory).State = EntityState.Modified;
        //    try
        //    {
        //        await _dbContext.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        throw;
        //    }
        //    return true;
        //}

        //public async Task<bool> DeleteBlobInventoryAsync(int id)
        //{
        //    BlobsInventory? blobsInventory = _dbContext.BlobsInventories.FirstOrDefault(x => x.IdBlobsInventory == id);
        //    if (blobsInventory == null)
        //        return false;
        //    _dbContext.BlobsInventories.Remove(blobsInventory);
        //    await _dbContext.SaveChangesAsync();
        //    return true;
        //}
    }
}