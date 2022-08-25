using Microsoft.AspNetCore.Mvc;
using Simstagram2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Simstagram2._0.Controllers
{
    public class RegisterController : Controller
    {
        private UserContext _context;
        public RegisterController(UserContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                using(var context = new UserContext())
                {
                    context.Database.EnsureCreated();
                    var userInfo = new User
                    {
                        FirstName = registerViewModel.FirstName,
                        LastName = registerViewModel.LastName,
                        Email = registerViewModel.Email,
                        Username = registerViewModel.Username,
                        Password = registerViewModel.Password
                    };
                    context.Users.Add(userInfo);
                    context.SaveChanges();

                    
                }
            }
            return RedirectToAction("Index", "Login");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
