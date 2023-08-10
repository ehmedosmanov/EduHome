﻿using EduHome3.DAL;
using EduHome3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome3.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDbContext _db;
        public CoursesController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            List<Course> courses = await _db.Courses.OrderByDescending(x => x.Id).Take(6).ToListAsync();
            ViewBag.CoursesCount = await _db.Courses.CountAsync();
            return View(courses);
        }

        public async Task<IActionResult> LoadMore(int skip)
        {
            int count = await _db.Courses.CountAsync();

            if(count <= skip)
            {
                return Content("Cix get ə");
            }

            List<Course> courses = await _db.Courses.
                                                    OrderByDescending(x => x.Id).
                                                    Skip(skip).
                                                    Take(6).
                                                    ToListAsync();

            return PartialView("_LoadMoreCoursesPartial", courses);
        }

        public async Task<IActionResult> Search(string key)
        {
            List<Course> courses = await _db.Courses.Where(x => x.Name.Contains(key)).ToListAsync();
            ViewBag.AllCourses = await _db.Courses.ToListAsync();
            return PartialView("_SearchCoursesPartial", courses);
        }
    }
}
