using EduHome3.DAL;
using EduHome3.Helpers;
using EduHome3.Models;
using EduHome3.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome3.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class FeedbackController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public FeedbackController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

       public async Task<IActionResult> Index()
        {
     
        List<Feedback> feedbacks = await _db.Feedbacks.ToListAsync();
            return View(feedbacks);
        }

   
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Feedback feedback)
        {
            if(feedback.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please Selecet a Photo");
                return View();
            }
            if (!feedback.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Please Upload a Photo");
                return View();
            }
            if (feedback.Photo.IsOlder1MB())
            {
                ModelState.AddModelError("Photo", "Max 1Mb");
                return View();
            }

            string folder = Path.Combine(_env.WebRootPath, "img", "testimonial");
            feedback.AuthorImg = await feedback.Photo.SaveFileAsync(folder);
            await _db.Feedbacks.AddAsync(feedback);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Update(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Feedback dbfeedback = await _db.Feedbacks.Where(x => x.Id == id).FirstOrDefaultAsync();

            if(dbfeedback == null)
            {
                return BadRequest();
            }
            return View(dbfeedback);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,Feedback feedback)
        {
            if (id == null)
            {
                return NotFound();
            }

            Feedback dbfeedback = await _db.Feedbacks.Where(x => x.Id == id).FirstOrDefaultAsync();

            if(feedback.Photo != null)
            {
                if (!feedback.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please Upload a Photo");
                    return View();
                }
                if (feedback.Photo.IsOlder1MB())
                {
                    ModelState.AddModelError("Photo", "Max 1mb");
                    return View();
                }

                string folder = Path.Combine(_env.WebRootPath, "img", "testimonial");
                string fullPath = Path.Combine(folder, dbfeedback.AuthorImg);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            if (dbfeedback == null)
            {
                return BadRequest();
            }

            dbfeedback.Author = feedback.Author;
            dbfeedback.Role = feedback.Role;
            dbfeedback.Description = feedback.Description;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Feedback dbfeedback = await _db.Feedbacks.Where(x => x.Id == id).FirstOrDefaultAsync();

            if(dbfeedback.IsDeactive) // atkiv deyil true
            {
                dbfeedback.IsDeactive = false; // aktiv ele
            } else
            {
                dbfeedback.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
