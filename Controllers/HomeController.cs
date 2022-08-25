using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Simstagram2._0.Models;

namespace Simstagram2._0.Controllers
{
    public class HomeController : Controller
    {
        private UserContext _context;
        private readonly ILogger<HomeController> _logger;
        private string _path;
        private static bool _isLiked = false;
        private static int _score;

        public static void InsertPost(PhotoViewModel photoViewModel, UserContext context, string username)
        {
            var searchForUser = context.Users.FirstOrDefault(x => x.Email == username);
            var photo = new Photo
            {
                FilePath = photoViewModel.Photo.FileName,
                Description = photoViewModel.Description,
                Location = new Location
                {
                    Name = photoViewModel.Localization
                },
                Title = photoViewModel.Title,
                Album = new Album
                {
                    Title = photoViewModel.Title,
                    Description = photoViewModel.Description
                },
                Interactions = new List<Interaction>(),
                WhoAdded = username,
                UploadDate = DateTime.Now
            };
            var interactions = new Interaction();
            interactions.Photo = photo;
            interactions.UserId = searchForUser.Id;
            photo.Interactions.Add(interactions);
            context.Photos.Add(photo);
            context.SaveChanges();
        }
        public HomeController(ILogger<HomeController> logger, UserContext context)
        {
            _logger = logger;
            _context = context;
            _path = @"C:\Users\Dell\source\repos\Simstagram2.0\wwwroot\img\";
        }
        public IActionResult Index(string search = null)
        {
            _context.Database.EnsureCreated();
            if (search != null)
            {
                //var searching = _context.Users.Where(x => x.Username.StartsWith(search) || x.FirstName.StartsWith(search)).ToList();
                //Lista użytkowników powyżej, to musiałby być osobny widok
                var searching = _context.Users.FirstOrDefault(x => x.Username == search);
                return RedirectToAction("Index", "Profile", new { username = searching.Username });
            }
            var searchForUser = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            if (searchForUser == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var searchAnotherUsers = _context.Users.Where(x => x.Username != User.Identity.Name).ToList();
            //var searchAnotherUsers = _context.Users;
            ViewData["Userowie"] = searchAnotherUsers;
            //lista userów, których followuje użytkownik
            List<ProfileViewModel> userPhotos = new List<ProfileViewModel>();
            var idAuthentity = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            var userFollows = _context.Follows.Where(user => user.UserId == idAuthentity.Id).ToList();
            userFollows.Distinct();

            foreach (var item in userFollows)
            {
                var u = _context.Users.FirstOrDefault(x => x.Id == item.FollowedId);
                var p = _context.Photos.Include(loc => loc.Location).Include(photo => photo.Interactions).Where(x => x.WhoAdded == u.Email);
                foreach (var tem in p)
                {
                    var pvm = new ProfileViewModel
                    {
                        Username = u.Username,
                        FilePath = tem.FilePath,
                        ProfilePhotoString = u.ProfilePhotoPath,
                        Description = tem.Description,
                        Title = tem.Title,
                        score = tem.Interactions.Where(x => x.PhotoId == tem.Id && x.Heart == true).Count()
                    };
                    if(tem.Location != null)
                    {
                        pvm.Location = tem.Location.Name;
                    }
                    else
                    {
                        pvm.Location = "Unknown";
                    }
                    
                    userPhotos.Add(pvm);
                }//kwerenda która wyszuka, któremu zdjęciu dodane zostaną lajki (sumowane)
            }
            ViewData["Photosy"] = userPhotos;
            



                return View();
        }
        public IActionResult UploadPost(PhotoViewModel photoViewModel)
        {
            using (var fileStream = new FileStream(Path.Combine(_path, $"{photoViewModel.Photo.FileName}"), FileMode.Create, FileAccess.Write))
            {
                photoViewModel.Photo.CopyTo(fileStream);
                using (var context = new UserContext())
                {
                    InsertPost(photoViewModel, context, User.Identity.Name);
                }
            }
            return View("Index");
        }
        public IActionResult Like(string filename)
        {
            var searchAuthenticationUser = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            var searchPhoto = _context.Photos.FirstOrDefault(x => x.FilePath == filename);
            if (_context.Interactions.Where(x => x.UserId == searchAuthenticationUser.Id & x.Photo.FilePath == filename).Count() == 0)
            {
                using (var context = new UserContext())
                {
                    var interaction = new Interaction();
                    interaction.UserId = searchAuthenticationUser.Id;
                    interaction.PhotoId = searchPhoto.Id;
                    interaction.Heart = true;
                    context.Interactions.Add(interaction);
                    context.SaveChanges();
                }
            }
            else
            {
                using (var context = new UserContext())
                {
                    //var interaction = new Interaction();
                    //interaction.UserId = searchAuthenticationUser.Id;
                    //interaction.PhotoId = searchPhoto.Id;
                    //interaction.Heart = false;
                    //context.Interactions.Update(interaction);
                    //context.SaveChanges();
                    var interaction = _context.Interactions.Where(x => x.UserId == searchAuthenticationUser.Id & x.Photo.FilePath == filename).First();
                    interaction.Heart = !interaction.Heart;
                    context.Update(interaction);
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Unlike(string filename)
        {
            var searchAuthenticationUser = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            var searchPhoto = _context.Photos.FirstOrDefault(x => x.FilePath == filename);
            if (_context.Interactions.Where(x => x.UserId == searchAuthenticationUser.Id & x.Photo.FilePath == filename).Count() != 0)
            {
                using (var context = new UserContext())
                {
                    var interaction = new Interaction();
                    interaction.Heart = false;
                    context.Interactions.Update(interaction);
                    context.SaveChanges();
                }
            }
            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
