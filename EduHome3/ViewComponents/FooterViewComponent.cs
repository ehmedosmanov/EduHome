using EduHome3.DAL;
using EduHome3.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EduHome3.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        private readonly AppDbContext _db;
        public FooterViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            FooterVM footerVM = new FooterVM()
            {
                Bios = await _db.Bios.FirstOrDefaultAsync(),
                SocialMedias = await _db.SocialMedias.ToListAsync()
            };  
            return View(footerVM);
        }
    }
}
