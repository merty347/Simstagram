using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Simstagram2._0.Models
{
    public class UserViewModel
    {
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        public IFormFile ProfilePhoto { get; set; }
        public string Email { get; set; }
        public int scoreHearts { get; set; }

    }
}
