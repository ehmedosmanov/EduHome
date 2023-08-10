using Microsoft.AspNetCore.Mvc;

namespace EduHome3.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
