using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simstagram2._0.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Simstagram2._0.Controllers
{
    public class ProfileController : Controller
    {
        private string _path;
        private UserContext _context;
        private static string _username;
        private static bool _isFollow = false;
        private static string _email;
        private static List<ProfileViewModel> profileViewModels = new List<ProfileViewModel>();
        
        
        public ProfileController(UserContext context)
        {
            _context = context;
            _path = @"C:\Users\Dell\source\repos\Simstagram2.0\wwwroot\img\";
        }
        //[HttpPost]
        //public IActionResult Index()
        //{
        //    return View();
        //}
        [HttpGet]
        public IActionResult Index(string username)
        {
            if (username != null)
            {
                _username = username;
            }
            var searchForEmail = _context.Users.FirstOrDefault(x => x.Username == username);            
            var searchAuthenticationUser = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            var searchFollowedUser = _context.Users.FirstOrDefault(x => x.Username == _username);
            if (username != null && searchFollowedUser.Username == _username)
            {
                if(_context.Follows.Where(x => x.UserId == searchAuthenticationUser.Id & x.FollowedId == searchFollowedUser.Id).Count() != 0)
                {
                    _isFollow = true;
                    ViewBag.isFollowed = _isFollow;
                }
                else
                {
                    _isFollow = false;
                    ViewBag.isFollowed = _isFollow;
                }

                var searchForUser = _context.Users
               .Include(user => user.Interactions)
               .ThenInclude(interaction => interaction.Photo)
               .FirstOrDefault(user => user.Username == _username);
                var searchForId = _context.Users.FirstOrDefault(x => x.Username == _username);
                _email = searchForEmail.Email;
                if (searchForEmail.Email == User.Identity.Name)
                {
                    ViewBag.Email = _email;
                }
                else
                {
                    ViewBag.Email = _email;
                }

                TempData["Username"] = searchForUser.Username;
                TempData["FirstName"] = searchForUser.FirstName;
                TempData["LastName"] = searchForUser.LastName;
                string sciezka = $@"/img/{searchForUser.ProfilePhotoPath}";
                TempData["Profilowe"] = sciezka;
                if (searchForUser.Description != null)
                {
                    TempData["Description"] = searchForUser.Description;
                }
                else
                {
                    TempData["Description"] = "Ten użytkownik nie dodał jeszcze opisu";
                }
                //Ładowanie zdjęć
                var searchForGallery = _context.Photos.Include(loc => loc.Location).Where(x => x.WhoAdded == searchForId.Email).ToList();
                //ViewData["GalleryUser"] = searchForGallery;
                profileViewModels.Clear();
                foreach (var item in searchForGallery)
                {
                    var heartCount = _context.Interactions.Where(x => x.PhotoId == item.Id && x.Heart == true).Count();
                    profileViewModels.Add(new ProfileViewModel 
                    { 
                        score = heartCount , 
                        FilePath = item.FilePath, 
                        Description = item.Description,
                        Title = item.Title,
                        Location = item.Location.Name, 
                        UploadTime = item.UploadDate,
                        Id = item.Id
                    });
                }
                ViewData["GalleryUser"] = profileViewModels;

                //Liczenie dodanych postów
                var searchForPhotos =_context.Photos.Where(x => x.WhoAdded == searchForId.Email).Count();
                TempData["CounterPhotos"] = searchForPhotos;

                //Wyświetlanie liczby followersów
                var userFollows = _context.Follows.Where(user => user.UserId == searchForId.Id).Count();
                TempData["CounterFollows"] = userFollows;

                //Wyświetlanie liczby followersów
                var userFollowers = _context.Follows.Where(user => user.FollowedId == searchForId.Id).Count();
                TempData["CounterFollowers"] = userFollowers;

                //Wyświetlanie liczby lajków do zdjęcia
                //var searchPhotoID = _context.Photos.FirstOrDefault(x => x.UploadDate = )
                //var likes = _context.Interactions.Where(p => p.PhotoId == )
            }
            var searchForAuthentityUser = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            if(username != null && searchForAuthentityUser.Username == _username)
            {
                var searchForId = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
                var searchForUser = _context.Users
               .Include(user => user.Interactions)
               .ThenInclude(interaction => interaction.Photo)
               .FirstOrDefault(user => user.Email == User.Identity.Name);
                ViewBag.User = searchForUser;
                TempData["Username"] = searchForUser.Username;
                TempData["FirstName"] = searchForUser.FirstName;
                TempData["LastName"] = searchForUser.LastName;
                string sciezka = $@"/img/{searchForUser.ProfilePhotoPath}";
                TempData["Profilowe"] = sciezka;
                if (searchForUser.Description != null)
                {
                    TempData["Description"] = searchForUser.Description;
                }
                else
                {
                    TempData["Description"] = "Ten użytkownik nie dodał jeszcze opisu";
                }
                //Ładowanie zdjęć
                var searchForGallery = _context.Photos.Include(loc => loc.Location).Where(x => x.WhoAdded == searchForId.Email).ToList();
                profileViewModels.Clear();
                foreach (var item in searchForGallery)
                {
                    var heartCount = _context.Interactions.Where(x => x.PhotoId == item.Id && x.Heart == true).Count();
                    profileViewModels.Add(new ProfileViewModel
                    {
                        score = heartCount,
                        FilePath = item.FilePath,
                        Description = item.Description,
                        Title = item.Title,
                        Location = item.Location.Name,
                        UploadTime = item.UploadDate,
                        Id = item.Id
                    });
                }
                ViewData["GalleryUser"] = profileViewModels;

                //Liczenie dodanych postów
                var searchForPhotos = _context.Photos.Where(x => x.WhoAdded == searchForId.Email).Count();
                TempData["CounterPhotos"] = searchForPhotos;

                //Wyświetlanie liczby followersów
                var idAuthentity = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
                var userFollows = _context.Follows.Where(user => user.UserId == idAuthentity.Id).Count();
                TempData["CounterFollows"] = userFollows;

                //Wyświetlanie liczby followersów
                var userFollowers = _context.Follows.Where(user => user.FollowedId == idAuthentity.Id).Count();
                TempData["CounterFollowers"] = userFollowers;

            }


            return View("Index");
        }        
        public IActionResult UserProfile()
        {
            var searchForId = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            var searchForUser = _context.Users
               .Include(user => user.Interactions)
               .ThenInclude(interaction => interaction.Photo)
               .FirstOrDefault(user => user.Email == User.Identity.Name);
            TempData["Username"] = searchForUser.Username;
            TempData["FirstName"] = searchForUser.FirstName;
            TempData["LastName"] = searchForUser.LastName;
            string sciezka = $@"/img/{searchForUser.ProfilePhotoPath}";
            TempData["Profilowe"] = sciezka;
            if (searchForUser.Description != null)
            {
                TempData["Description"] = searchForUser.Description;
            }
            else
            {
                TempData["Description"] = "Ten użytkownik nie dodał jeszcze opisu";
            }
            _email = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name).ToString();
            ViewBag.Email = _email;
            var userAuthentication = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            if (userAuthentication.Email != null)
            {
                _email = userAuthentication.Email;
                ViewBag.Email = _email;
            }
            //Ładowanie zdjęć            
            var searchForGallery = _context.Photos.Include(loc => loc.Location).Where(x => x.WhoAdded == searchForId.Email).ToList();
            profileViewModels.Clear();
            foreach (var item in searchForGallery)
            {
                var heartCount = _context.Interactions.Where(x => x.PhotoId == item.Id && x.Heart == true).Count();
                profileViewModels.Add(new ProfileViewModel
                {
                    score = heartCount,
                    FilePath = item.FilePath,
                    Description = item.Description,
                    Title = item.Title,
                    Location = item.Location.Name,
                    UploadTime = item.UploadDate,
                    Id = item.Id
                });
            }
            ViewData["GalleryUser"] = profileViewModels;

            //Liczenie dodanych postów
            var searchForPhotos = _context.Photos.Where(x => x.WhoAdded == searchForId.Email).Count();
            TempData["CounterPhotos"] = searchForPhotos;

            //Wyświetlanie liczby osób, ktr się obserwuje
            var userFollows = _context.Follows.Where(user => user.UserId == searchForId.Id).Count();
            TempData["CounterFollows"] = userFollows;

            //Wyświetlanie liczby followersów
            var userFollowers = _context.Follows.Where(user => user.FollowedId == searchForId.Id).Count();
            TempData["CounterFollowers"] = userFollowers;

            return View("Index");
        }
        public IActionResult AddProfilePic(UserViewModel userViewModel)
        {
            User user = new User();
            var searchForPhoto = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            if(searchForPhoto.ProfilePhotoPath == null)
            {
                using (var fileStream = new FileStream(Path.Combine(_path, $"{userViewModel.ProfilePhoto.FileName}"), FileMode.Create, FileAccess.Write))
                {
                    userViewModel.ProfilePhoto.CopyTo(fileStream);
                    using(var context = new UserContext())
                    {
                        user = context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
                        user.ProfilePhotoPath = $"{userViewModel.ProfilePhoto.FileName}";
                        context.SaveChanges();
                    }
                }
            }
            TempData["Dodano"] = "Dodano zdj profilowe";
            return View("Index");
        }
        [HttpGet]
        public IActionResult EditPost(string location, string desc, string title, int id, DateTime uploadDate)
        {
            Photo photo = new Photo();
            var searchForUser = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            if (searchForUser.Email == User.Identity.Name)
            {
                using(var contex = new UserContext())
                {
                    photo = contex.Photos.Include(loc => loc.Location).FirstOrDefault(x => x.Id == id);
                    photo.Location.Name = location;
                    photo.Description = desc;
                    photo.Title = title;
                    photo.UploadDate = uploadDate;
                    contex.SaveChanges();
                }
                return RedirectToAction("EditPhoto", "Profile", new { photoId = photo.Id});
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
                
        }
        [HttpPost]
        public IActionResult EditPost(PhotoViewModel photoViewModel)
        {
            Photo photo = new Photo();
            var searchForUser = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            if (searchForUser.Email == User.Identity.Name)
            {
                using (var contex = new UserContext())
                {
                    photo = contex.Photos.Include(loc => loc.Location).FirstOrDefault(x => x.UploadDate == photoViewModel.UploadDate);
                    photo.Location.Name = photoViewModel.Localization;
                    photo.Description = photoViewModel.Description;
                    photo.Title = photoViewModel.Title;
                    contex.SaveChanges();
                }
                //return RedirectToAction("EditPhoto", "Profile", new { photoId = photo.Id });
                return RedirectToAction("UserProfile", "Profile");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public IActionResult EditPhoto(int photoId)
        {
            //var searchUser = _context.Photos.FirstOrDefault(x => x.WhoAdded == User.Identity.Name);
            //ViewBag.Email = searchUser;
            var photo = _context.Photos.Include(loc => loc.Location).FirstOrDefault(x => x.Id == photoId);
            var su = photo.WhoAdded;
            if (su != User.Identity.Name)
            {
                return RedirectToAction("Index", "Home");
            }
            var pvm = new PhotoViewModel
            {
                Title = photo.Title,
                Description = photo.Description,
                PhotoString = photo.FilePath,
                Localization = photo.Location.Name,
                UploadDate = photo.UploadDate
            };
            return View(pvm);
        }
        public IActionResult Follow()
        {
            var searchAuthenticationUser = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            var searchFollowedUser = _context.Users.FirstOrDefault(x => x.Username == _username);
            if (_context.Follows.Where(x => x.UserId == searchAuthenticationUser.Id & x.FollowedId == searchFollowedUser.Id).Count() == 0)
            {
                using (var context = new UserContext())
                {
                    var follow = new Follow();
                    follow.UserId = searchAuthenticationUser.Id;
                    follow.FollowedId = searchFollowedUser.Id;
                    context.Follows.Add(follow);
                    context.SaveChanges();
                }
            }
            return View("Index");
        }
        public IActionResult Unfollow()
        {
            var searchAuthenticationUser = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            var searchFollowedUser = _context.Users.FirstOrDefault(x => x.Username == _username);
            if (_context.Follows.Where(x => x.UserId == searchAuthenticationUser.Id & x.FollowedId == searchFollowedUser.Id).Count() != 0)
            {
                using (var context = new UserContext())
                {
                    var following = context.Follows.FirstOrDefault(x => x.UserId == searchAuthenticationUser.Id & x.FollowedId == searchFollowedUser.Id);
                    context.Follows.Remove(following);
                    context.SaveChanges();
                }
            }
            return View("Index");
        }
        [HttpGet]
        public IActionResult EditProfile()
        {
            User user = new User();
            var searchUser = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            if(searchUser.Email == User.Identity.Name)
            {
                using(var context = new UserContext())
                {
                    user = context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
                    user.FirstName = searchUser.FirstName;
                    user.LastName = searchUser.LastName;
                    user.Description = searchUser.Description;
                }
            }
            return View("Edit");
        }
        [HttpPost]
        public IActionResult EditProfile(UserViewModel userViewModel)
        {
            User user = new User();
            var searchForUser = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            if(searchForUser.Email == User.Identity.Name)
            {
                using (var context = new UserContext())
                {
                    user = context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
                    user.FirstName = userViewModel.FirstName;
                    user.LastName = userViewModel.LastName;
                    user.Description = userViewModel.Description;
                    context.SaveChanges();
                }
                return RedirectToAction("UserProfile", "Profile");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult EditProfilePic(UserViewModel userViewModel)
        {
            User user = new User();
            var searchForPhoto = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            using (var fileStream = new FileStream(Path.Combine(_path, $"{userViewModel.ProfilePhoto.FileName}"), FileMode.Create, FileAccess.Write))
            {
                userViewModel.ProfilePhoto.CopyTo(fileStream);
                using (var context = new UserContext())
                {
                    user = context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
                    user.ProfilePhotoPath = $"{userViewModel.ProfilePhoto.FileName}";
                    context.SaveChanges();
                }
            }
            return View("Index");
        }
    }
}
