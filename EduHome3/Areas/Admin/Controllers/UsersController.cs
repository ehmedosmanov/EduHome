using EduHome3.DAL;
using EduHome3.Helpers;
using EduHome3.Models;
using EduHome3.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome3.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _db;

        public UsersController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }


        public async Task<IActionResult> Index()
        {
            List<AppUser> appUsers = await _userManager.Users.ToListAsync();
            List<UserVM> usersVM = new List<UserVM>();

            foreach (AppUser user in appUsers)
            {
                UserVM userVM = new UserVM
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Surname = user.Surname,
                    Username = user.UserName,
                    IsDeactive = user.IsDeactive,
                    Role = (await _userManager.GetRolesAsync(user))[0]
                };
                usersVM.Add(userVM);
            }

            return View(usersVM);
        }

        #region Activity
        public async Task<IActionResult> Activity(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser dbUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (dbUser == null)
            {
                return BadRequest();
            }
            if (dbUser.IsDeactive)
            {
                dbUser.IsDeactive = false;
            }
            else
            {
                dbUser.IsDeactive = true;
            }
            await _userManager.UpdateAsync(dbUser);
            return RedirectToAction("Index");
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            ViewBag.Roles = new List<string>
            {
                Helper.Admin,
                Helper.Member
            };

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterVM registerVM, string role)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            ViewBag.Roles = new List<string>
            {
                Helper.Admin,
                Helper.Member
            };

            AppUser appUser = new AppUser
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
                Email = registerVM.Email
            };

            IdentityResult identityResult = await _userManager.CreateAsync(appUser, password: registerVM.Password);

            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }

            await _userManager.AddToRoleAsync(appUser, role);
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            UpdateVM updateVM = new UpdateVM
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Username = user.UserName,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(),
            };
            ViewBag.Roles = new List<string>
            {
                Helper.Admin,
                Helper.Member
            };
            return View(updateVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, UpdateVM updateVM, string role)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            UpdateVM dbUpdateVM = new UpdateVM
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Username = user.UserName,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
            };

            ViewBag.Roles = new List<string>
            {
                Helper.Admin,
                Helper.Member
            };

            if (!ModelState.IsValid)
            {
                return View(dbUpdateVM);
            }

            user.Email = updateVM.Email;
            user.UserName = updateVM.Username;
            user.Name = updateVM.Name;
            user.Surname = updateVM.Surname;

            await _userManager.UpdateAsync(user); 

            if (role != dbUpdateVM.Role)    
            {
                IdentityResult addIdentityResult = await _userManager.AddToRoleAsync(user, role);
                if (!addIdentityResult.Succeeded)
                {
                    foreach (IdentityError error in addIdentityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
                IdentityResult removeIdentityResult = await _userManager.RemoveFromRoleAsync(user, role: dbUpdateVM.Role);
                if (!removeIdentityResult.Succeeded)
                {
                    foreach (IdentityError error in removeIdentityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region ResetPassword
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser appUser = await _userManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return BadRequest();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM, string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser appUser = await _userManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return BadRequest();
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
            IdentityResult identityResult = await _userManager.ResetPasswordAsync(appUser, token, resetPasswordVM.Password);

            if(!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            return RedirectToAction("Index");
        }

        #endregion


        #region Delete

        public async Task<IActionResult> Delete(string id)
        {
            AppUser userToDelete = await _userManager.FindByIdAsync(id);
            if (userToDelete == null)
            {
                return NotFound();
            }

            UserVM userVM = new()
            {
                Id = userToDelete.Id,
                Username = userToDelete.UserName,
                Email = userToDelete.Email
            };
            return View(userVM);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            AppUser userToDelete = await _userManager.FindByIdAsync(id);
            if (userToDelete == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userManager.DeleteAsync(userToDelete);

            if (!result.Succeeded)
            {
                // Обработка ошибок при удалении, если необходимо
                return View("Delete", new UserVM
                {
                    Id = userToDelete.Id,
                    Username = userToDelete.UserName,
                    Email = userToDelete.Email
                });
            }

            return RedirectToAction("Index");
        }

        #endregion

    }
}
