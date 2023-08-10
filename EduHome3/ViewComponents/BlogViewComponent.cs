using EduHome3.DAL;
using EduHome3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome3.ViewComponents
{
    public class BlogViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;
        public BlogViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync(int count)
        {
            List<Blog> blogs = await _db.Blogs.Take(count).ToListAsync();

            return View(blogs);
        }
    }
}
