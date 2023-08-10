using Microsoft.AspNetCore.Mvc;

namespace EduHome3.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
