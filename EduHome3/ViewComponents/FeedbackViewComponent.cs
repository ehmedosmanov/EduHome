using EduHome3.DAL;
using EduHome3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome3.ViewComponents
{
    public class FeedbackViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;
        public FeedbackViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Feedback> feedbacks = await _db.Feedbacks.Where(x => !x.IsDeactive).ToListAsync();

            return View(feedbacks);
        }
    }
}
