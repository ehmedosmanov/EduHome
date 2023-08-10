using EduHome3.DAL;
using EduHome3.Helpers;
using EduHome3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EduHome3.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SlidersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public SlidersController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> slider = await _db.Sliders.ToListAsync();
            return View(slider);
        }  
        
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            

            Slider dbslider = await _db.Sliders.FirstOrDefaultAsync(dbs => dbs.Id == id);

            if (dbslider == null)
            {
                return BadRequest();
            }

            if (dbslider.IsDeactive)
            {
                dbslider.IsDeactive = false;
            }
            else
            {
                dbslider.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create(Slider slider)
        {
            if(slider.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please Selecet a Photo");
                return View();
            }
            if(!slider.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Please Upload a Photo");
                return View();
            }
            if(slider.Photo.IsOlder1MB())
            {
                ModelState.AddModelError("Photo", "Max 1Mb");
                return View();
            }

            string folder = Path.Combine(_env.WebRootPath, "img", "slider");
            slider.Img = await slider.Photo.SaveFileAsync(folder);

            await _db.Sliders.AddAsync(slider);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Slider dbslider = await _db.Sliders.FirstOrDefaultAsync(dbs => dbs.Id == id);

            if(dbslider == null)
            {
                return BadRequest();
            }

            return View(dbslider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Slider slider)
        {
            if (id == null)
            {   
                return NotFound();
            }


            Slider dbslider = await _db.Sliders.FirstOrDefaultAsync(dbs => dbs.Id == id);

            if (slider.Photo != null)
            {
                if(!slider.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please Upload a Photo");
                    return View();
                }
                if(slider.Photo.IsOlder1MB())
                {
                    ModelState.AddModelError("Photo", "Max 1mb");
                    return View();
                }

                string folder = Path.Combine(_env.WebRootPath, "img", "slider");
                string fullPath = Path.Combine(folder, dbslider.Img);
                if(System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                dbslider.Img = await slider.Photo.SaveFileAsync(folder);
            }

            if (dbslider == null)
            {
                return BadRequest();
            }


            dbslider.Title = slider.Title;
            dbslider.Description = slider.Description;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
