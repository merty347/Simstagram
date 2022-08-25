using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Simstagram2._0.Models
{
    public class User
    {
        public int Id { get; set; }
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        public string Email { get; set; }
        [Display(Name="Username")]
        public string Username { get; set; }
        public string ProfilePhotoPath { get; set; }
        public ICollection<Follow> Follows { get; set; }
        public ICollection<Interaction> Interactions { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }


    }
}
