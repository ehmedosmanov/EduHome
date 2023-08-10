using EduHome3.DAL;
using EduHome3.Models;
using EduHome3.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome3.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db = db;       
        }
        public async Task<IActionResult> Index()
        {   
         
            HomeVM homeVM = new HomeVM
            {
                sliders = await _db.Sliders.Where(x => !x.IsDeactive).ToListAsync(),
                services = await _db.Services.Where(x => !x.IsDeactive).Skip(1).Take(3).OrderByDescending(x => x.Name).ToListAsync()
                
            };
            return View(homeVM);
        }
    }
}
