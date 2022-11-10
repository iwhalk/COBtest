using Microsoft.EntityFrameworkCore;
using ReportesInmobiliaria.Interfaces;
using SharedLibrary.Data;
using SharedLibrary.Models;

namespace ReportesInmobiliaria.Services
{
    public class BlobService : IBlobService
    {
        private readonly InmobiliariaDbContext _dbContext;

        public BlobService(InmobiliariaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Blob>?> GetBlobAsync()
        {
            return await _dbContext.Blobs.ToListAsync();
        }

        public async Task<Blob?> CreateBlobAsync(Blob blob)
        {
            await _dbContext.Blobs.AddAsync(blob);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return blob;
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

        public async Task<List<BlobsInventory>?> GetBlobInventoryAsync()
        {
            return await _dbContext.BlobsInventories.ToListAsync();
        }

        public async Task<BlobsInventory?> CreateBlobInventoryAsync(BlobsInventory blobsInventory)
        {
            await _dbContext.BlobsInventories.AddAsync(blobsInventory);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return blobsInventory;
        }

        public async Task<bool> UpdateBlobInventoryAsync(BlobsInventory blobsInventory)
        {
            _dbContext.Entry(blobsInventory).State = EntityState.Modified;
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

        public async Task<bool> DeleteBlobInventoryAsync(int id)
        {
            BlobsInventory? blobsInventory = _dbContext.BlobsInventories.FirstOrDefault(x => x.IdBlobsInventory == id);
            if (blobsInventory == null)
                return false;
            _dbContext.BlobsInventories.Remove(blobsInventory);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}