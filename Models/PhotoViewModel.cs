using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Simstagram2._0.Models
{
    public class PhotoViewModel
    {
        [Display(Name = "Tytuł")]
        public string Title { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        public DateTime UploadDate { get; set; }
        public IFormFile Photo { get; set; }
        public string PhotoString { get; set; }
        [Display(Name = "Lokalizacja")]
        public string Localization { get; set; }
        public bool Heart { get; set; }
    }
}
