using Microsoft.AspNetCore.Mvc;

namespace EduHome3.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
