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
        private readonly IBlobsService _blobsService;

        public BlobsController(IBlobsService blobsService)
        {
            _blobsService = blobsService;
        }

        [HttpGet]
        public async Task<ActionResult> GetBlobs()
        {
            var result = await _blobsService.GetBlobsAsync();

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<ActionResult> PostBlob(Blob blob)
        {
            var result = await _blobsService.PostBlobAsync(blob);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
