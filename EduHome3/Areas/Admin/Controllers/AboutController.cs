using EduHome3.DAL;
using EduHome3.Helpers;
using EduHome3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome3.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class AboutController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public AboutController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            About about = await _db.Abouts.FirstOrDefaultAsync();
            return View(about);
        }



        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            About dbabout = await _db.Abouts.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (dbabout == null)
            {
                return BadRequest();
            }

            return View(dbabout);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,About about)
        {
            if(id == null)
            {
                return NotFound();
            }

            About dbabout = await _db.Abouts.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (about.Photo != null)
            {
                if (!about.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please Upload a Photo");
                    return View();
                }
                if (about.Photo.IsOlder1MB())
                {
                    ModelState.AddModelError("Photo", "Max 1mb");
                    return View();
                }

                string folder = Path.Combine(_env.WebRootPath, "img", "about");
                string fullPath = Path.Combine(folder, dbabout.Img);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                dbabout.Img = await about.Photo.SaveFileAsync(folder);
            }

            if (dbabout == null)
            {
                return BadRequest();
            }

            dbabout.Title = about.Title;
            dbabout.Description = about.Description;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }
}
