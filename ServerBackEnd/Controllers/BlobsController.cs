using ApiGateway.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
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
        public async Task<ActionResult> PostBlob(Blob blob)
        {
            var result = await _blobService.PostBlobAsync(blob);

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

        [HttpDelete("/{idBlob}")]
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
