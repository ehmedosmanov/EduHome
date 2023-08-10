using EduHome3.DAL;
using EduHome3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome3.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _db;
        public BlogController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _db.Blogs.ToListAsync();

            return View(blogs);
        }

        public async Task<IActionResult> Search(string key)
        {
            List<Blog> blogs = await _db.Blogs.Where(x => x.Name.Contains(key)).ToListAsync();
            ViewBag.AllBlogs = await _db.Blogs.ToListAsync();
            

            return PartialView("_SearchBlogPartial", blogs);
        }
    }
}


