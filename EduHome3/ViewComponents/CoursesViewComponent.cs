using EduHome3.DAL;
using EduHome3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome3.ViewComponents
{
    public class CoursesViewComponent:ViewComponent
    {
        private readonly AppDbContext _db;

        public CoursesViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync(int count)
        {
            List<Course> courses = await _db.Courses.OrderByDescending(x => x.Id).Take(count).ToListAsync();

            return View(courses);
        }
    }
}
