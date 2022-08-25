using Microsoft.AspNetCore.Mvc;
using Simstagram2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simstagram2._0.Controllers
{
    public class EditController : Controller
    {
        private UserContext _context;
        public IActionResult Index(UserContext context)
        {
            _context = context;
            return View();
        }
        public IActionResult EditProfile(UserViewModel userViewModel)
        {
            User user = new User();
            var searchForUser = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            using (var context = new UserContext())
            {
                user = context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
                user.FirstName = userViewModel.FirstName;
                user.LastName = userViewModel.LastName;
                user.Description = userViewModel.Description;
                context.SaveChanges();
            }
            return RedirectToAction("Index", "Profile");
        }
    }
}
