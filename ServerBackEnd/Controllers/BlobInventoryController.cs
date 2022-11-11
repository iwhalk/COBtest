using ApiGateway.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BlobInventoryController : ControllerBase
    {
        private readonly IBlobInventoryService _blobInventoryService;
        public BlobInventoryController(IBlobInventoryService blobInventoryService)
        {
            _blobInventoryService = blobInventoryService;
        }

        [HttpGet]
        public async Task<ActionResult> GetBlobInventory()
        {
            var result = await _blobInventoryService.GetBlobsInventoryAsync();

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<ActionResult> PostBlobInventory(BlobsInventory blobs)
        {
            var result = await _blobInventoryService.PostBlobInventoryAsync(blobs);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPut]
        public async Task<ActionResult> PutBlobInventory(BlobsInventory blobs)
        {
            var result = await _blobInventoryService.PutBlobInventoryAsync(blobs);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpDelete("{idBlob}")]
        public async Task<ActionResult> DeleteInventory(int idBlob)
        {
            var result = await _blobInventoryService.DeleteBlobInventoryAsync(idBlob);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
