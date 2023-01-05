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
        [HttpGet("Image/{id}")]
        public async Task<ActionResult> GetBlobImage(string id)
        {
            int idInt = 0;

            if (id != null)
            {
                idInt = Convert.ToInt16(id);
            }

            var result = await _blobsService.GetBlobImage(idInt);

            if (result.Succeeded)
            {
                return File(result.Content, result.ContentType.Equals("image/jpeg") == false ? "image/jpeg" : "image/jpeg");
            }

            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBlob(string id)
        {
            int idInt = 0;

            if (id != null)
            {
                idInt = Convert.ToInt16(id);
            }

            var result = await _blobsService.GetBlobAsync(idInt);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetBlobs(string? id)
        {
            int idInt = 0;

            if (id != null)
            {
                idInt = Convert.ToInt16(id);
            }

            var result = await _blobsService.GetBlobsAsync(idInt);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<ActionResult> PostBlob(IFormFile file)
        {
            var result = await _blobsService.PostBlobAsync(new() { BlobSize = file.Length.ToString(), ContentType = file.ContentType, BlobTypeId = "1" }, file);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
