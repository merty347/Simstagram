using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Simstagram2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Simstagram2._0.Controllers
{
    public class LoginController : Controller
    {
        private UserContext _context;
        public LoginController(UserContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Login(LoginViewModel loginVModel, string returnUrl) 
        {
            
            if (ModelState.IsValid)
            {
                //wyszukiwanie w bazie danych
                //var user = true;
                var user = _context.Users.FirstOrDefault(x => x.Email == loginVModel.Email && x.Password == loginVModel.Password);
                if(user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, loginVModel.Email)
                        //new Claim("Username", loginVModel.Username)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authentProps = new AuthenticationProperties
                    {
                        IsPersistent = true //zmienić, bo inaczej ciągle zalogowany jesteś 
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authentProps);
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Login");
            
        }
        public IActionResult Index()
        {
            _context.Database.EnsureCreated();
            var searchForUser = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            if (searchForUser != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
