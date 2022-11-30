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
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivitiesService _activitiesService;

        public ActivitiesController(IActivitiesService activitiesService)
        {
            _activitiesService = activitiesService;
        }

        [HttpGet]
        public async Task<ActionResult> GetActivities()
        {
            var result = await _activitiesService.GetActivitiesAsync();

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<ActionResult> PostActivity(Activity activity)
        {
            var result = await _activitiesService.PostActivityAsync(activity);

            if (result.Succeeded)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
