using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Simstagram2._0.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Display(Name = "Nazwa Użytkownika")]
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
    }
    public class SimstagramUserValidator : AbstractValidator<User>
    {
        public SimstagramUserValidator()
        {
            RuleFor(x => x.Username).NotNull();
            RuleFor(x => x.Password).NotNull().WithMessage("Password can not be empty");
            RuleFor(x => x.Email).NotNull().EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible);
        }
    }
}
