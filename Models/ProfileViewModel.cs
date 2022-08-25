using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Simstagram2._0.Models
{
    public class ProfileViewModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }
        public IFormFile ProfilePhoto { get; set; }
        public string ProfilePhotoString { get; set; }
        public string FilePath { get; set; } //dotyczy zdjęcia dodanego przez użytkownika
        [Display(Name = "Lokalizacja")]
        public string? Location { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int score { get; set; }
        public int Id { get; set; } //dotyczy zdjęcia
        public DateTime UploadTime { get; set; }
    }
}
