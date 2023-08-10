using EduHome3.DAL;
using EduHome3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduHome3.ViewComponents
{
    public class NoticeBoardViewComponent:ViewComponent
    {
        private readonly AppDbContext _db;
        public NoticeBoardViewComponent(AppDbContext db)
        {
            _db = db;
        } 
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<NoticeBoard> noticeBoards = await _db.NoticeBoards.ToListAsync();
            return View(noticeBoards);
        }
    }
}
