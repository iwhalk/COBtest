using ApiGateway.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BlobsController : ControllerBase
    {
        private readonly IBlobsService _blobService;
        public BlobsController(IBlobsService blobsService)
        {
            _blobService = blobsService;
        }

        [HttpGet]
        public async Task<ActionResult> GetBlob()
        {
            var result = await _blobService.GetBlobsAsync();

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<ActionResult> PostBlob(IFormFile file)
        {
            var result = await _blobService.PostBlobAsync(new() { BlobSize = file.Length.ToString(), ContentType = file.ContentType, BlodTypeId = "1"}, file);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPut]
        public async Task<ActionResult> PutBlob(Blob blob)
        {
            var result = await _blobService.PutBlobAsync(blob);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpDelete("{idBlob}")]
        public async Task<ActionResult> Delete(int idBlob)
        {
            var result = await _blobService.DeleteBlobAsync(idBlob);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
