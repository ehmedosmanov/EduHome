using EduHome3.DAL;
using EduHome3.Helpers;
using EduHome3.Models;
using EduHome3.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome3.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        #region Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            //firstordefault kimi isleyir null qayidacaq sehv islese 
            AppUser appUser = await _userManager.FindByNameAsync(loginVM.Username);//Name Usernamein name i dir yeni usernamedir

            if (appUser == null) //eger null olsa username email ile yoxla 
            {
                appUser = await _userManager.FindByEmailAsync(loginVM.Username); //email de null olsa sehv i qaytar
                if (appUser == null)
                {
                    ModelState.AddModelError("", "email ve username sehvdir");
                    return View();
                }
            }

            if(appUser.IsDeactive)
            {
                ModelState.AddModelError("", "Deaktive");
                return View();
            }

            //password sign in 
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, true, true);

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Limiti keçmisiniz");
                return View();
            }

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "İstifadəçi adı və şifrə səhvdir!");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion


        #region Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

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


            await _userManager.AddToRoleAsync(appUser, Helper.Admin);
            await _signInManager.SignInAsync(appUser, true);

            return RedirectToAction("Index", "Home");
        }
        #endregion



        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {

            AppUser user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "bele email yoxdu");
                return View();
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);


            string callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, Token = token }, HttpContext.Request.Scheme, "localhost:44376");

            string body = $"Zəhmət olmasa, aşağıdakı linkə klikləməklə parolunuzu sıfırlayın: <a href='{callbackUrl}'>Reset Password</a>";


            await Helper.SendMail(callbackUrl, model.Email);



            TempData["ConfirmationMessage"] = "mektub gonderildi emaile";
            return RedirectToAction(nameof(ForgotPassword));
        }




        #region ResetPassword
        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return NotFound();
            }

            AppUser appUser = await _userManager.FindByIdAsync(userId);

            if (appUser == null)
            {
                return BadRequest();
            }

            ResetPasswordVM model = new ResetPasswordVM
            {
                Id = userId,
                Token = token
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string userId, string token, ResetPasswordVM resetPasswordVM)
        {
            if (userId == null || token == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(resetPasswordVM);
            }

            AppUser appUser = await _userManager.FindByIdAsync(userId);

            if (appUser == null)
            {
                return BadRequest();
            }

            IdentityResult identityResult = await _userManager.ResetPasswordAsync(appUser, token, resetPasswordVM.Password);

            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(resetPasswordVM);
            }

            return RedirectToAction("Login", "Account");
        }

        #endregion







        #region Create Roles
        public async Task CreateRoles()
        {
            if (!await _roleManager.RoleExistsAsync(Helper.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Admin });
            }


            if (!await _roleManager.RoleExistsAsync(Helper.Member))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Member });
            }

        }
        #endregion

        
        
        #region LogOut
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion



    }
}
