using EduHome3.DAL;
using EduHome3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EduHome3.ViewComponents
{
    public class VideoTourViewComponent:ViewComponent
    {
        private readonly AppDbContext _db;
        public VideoTourViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync() 
        {
            VideoTour video = await _db.VideoTours.FirstOrDefaultAsync();
            return View(video); 
        }    
    }
}
