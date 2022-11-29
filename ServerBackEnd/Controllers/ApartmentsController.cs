using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    public class ApartmentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
