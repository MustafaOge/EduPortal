using Microsoft.AspNetCore.Mvc;

namespace EduPortal.Controllers
{
    public class ConsumerController : Controller
    {
        public IActionResult Consumer()
        {
            return View();
        }
    }
}
