using EduHome3.DAL;
using EduHome3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome3.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class ServicesController : Controller
    {
        private readonly AppDbContext _db;
        public ServicesController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            List<Service> services = await _db.Services.ToListAsync();
            return View(services);
        }

        #region Create
        public IActionResult Create() //Get Action
        {
            return View();
        }
        //Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service) // Post Action
        {

            #region Exist
            //AnyAsync yoxlayir oldugunu
            bool isExist = await _db.Services.AnyAsync(dbservice => dbservice.Name == service.Name); //Eyni adli servis olmasini yoxlayir
            if (isExist)
            {
                ModelState.AddModelError("Name", "This service already exists"); //eger varsa error

                return View();
            }
            #endregion 
            await _db.Services.AddAsync(service);
            await _db.SaveChangesAsync(); //sql-e gedir save
            return RedirectToAction("Index");  //Index-e qayit
        }
        #endregion



        #region Update
        //Update GET ACTION bize update basanda basdigimiz servisin id si gelir GET edir 
        public async Task<IActionResult> Update(int? id /*nulable*/) //asp-route-id = burdaki idye gelir     frontdan gelen basdigimiz id == olan servisi tap ve onu view ya apaririq
        {
            if (id == null)
            {
                return NotFound(); //id ni kimse silende not found verir 
            }
            Service dbService = await _db.Services.FirstOrDefaultAsync(x => x.Id == id); //bizim girdiyimiz servisin updatein id si gelir bura beraber edir eger true dusa view a gedir eger true deyilse null qaytarir o da badrequest e gedir

            if (dbService == null)
            {
                return BadRequest();
            }
            return View(dbService);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Service service)
        {
            if (id == null)
            {
                return NotFound();
            }
            Service dbService = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);

            if (dbService == null)
            {
                return BadRequest();
            }
            #region Exist
            bool isExist = await _db.Services.AnyAsync(x => x.Name == service.Name && x.Id != id);
            //databasedeki ID ler ne vaxtki beraber deyilse ve ad eynidise onda error qayidir id eyni olanda demiyecek bele ad var                                                                                 

            if (isExist)
            {
                ModelState.AddModelError("Name", "This service already exists");

                return View();
            }
            #endregion

            //deyisirik teze gelen datani kohne ile 
            dbService.Name = service.Name;
            dbService.Description = service.Description;
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        #endregion


        #region Detail
        //Detail
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Service dbService = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);

            if (dbService == null)
            {
                return BadRequest();
            }
            return View(dbService);
        }
        #endregion


        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            Service dbser = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);

            if (dbser == null)
            {
                return BadRequest();
            }
            return View(dbser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")] //action ad qoyur delete
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Service dbser = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);

            if (dbser == null)
            {
                return BadRequest();
            }
            dbser.IsDeactive = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Service dbser = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);

            if (dbser == null)
            {
                return BadRequest();
            }

            if(dbser.IsDeactive /*aktiv deyilse*/)
            {
                dbser.IsDeactive = false; /*aktiv e deyisir*/
            }
            else
            {
                dbser.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
