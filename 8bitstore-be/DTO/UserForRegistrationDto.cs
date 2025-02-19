using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace _8bitstore_be.DTO
{
    public class UserForRegistrationDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        public string FullName { get; set; }
        public string Address { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password does not match")]
        public string ConfirmPassword { get; set; }

    }
}
