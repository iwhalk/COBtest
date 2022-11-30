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
    public class BuildingsController : ControllerBase
    {
        private readonly IBuildingsService _buildingsService;

        public BuildingsController(IBuildingsService buildingsService)
        {
            _buildingsService = buildingsService;
        }

        [HttpGet]
        public async Task<ActionResult> GetBuildings()
        {
            var result = await _buildingsService.GetBuildingsAsync();

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<ActionResult> PostBuilding(Building building)
        {
            var result = await _buildingsService.PostBuildingAsync(building);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
