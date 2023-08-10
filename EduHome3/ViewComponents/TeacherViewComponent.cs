using EduHome3.DAL;
using EduHome3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome3.ViewComponents
{
    public class TeacherViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;
        public TeacherViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync(int count)
        {
            List<Teacher> teachers = await _db.Teachers.Take(count).ToListAsync();

            return View(teachers);
        }
    }
}
