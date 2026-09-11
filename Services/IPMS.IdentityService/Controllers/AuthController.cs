using Microsoft.AspNetCore.Mvc;

namespace IPMS.IdentityService.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
