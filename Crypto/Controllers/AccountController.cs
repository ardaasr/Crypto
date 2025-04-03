using Crypto.Identity;
using Crypto.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
         {
            _userManager = userManager;
            _signManager = signInManager;
         }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
               var result = await _signManager.PasswordSignInAsync(user, model.Password, model.IsPersient, true);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model) 
        {
            if (model.Password.Length<6) //burda hata mesajını böyle siz elle ekliyebilirsiniz.
            {
                ModelState.AddModelError("", "Şifreniz En Az 6 Karakterli Olmalıdır.");
                return View(model);
            }

            if (ModelState.IsValid) //IsValıd ifadesi RegisterViewModel de ki koşullar sağlanıyor mu onu kontrol eder , eğer sağlanıyorsa bu kullanılır.
            {
                ApplicationUser user = new ApplicationUser();
                user.Email=model.Email;
                user.FullName = model.Name + "" + model.Surname;
                user.UserName=model.Username;

                var result = await _userManager.CreateAsync(user,model.Password);

                if (result.Errors.Count() > 0) 
                {
                    foreach (var i in result.Errors) 
                    {
                        ModelState.AddModelError(i.Code, i.Description);
                    }
                    return View(model);
                }

                return RedirectToAction("Login");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
