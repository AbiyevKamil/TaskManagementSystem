using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManagementSystem.Models
{
    public class RegisterModel
    {
        [Required, DisplayName("Fullname")]
        public string FullName { get; set; }
        [Required, DisplayName("Username")]
        public string Username { get; set; }
        [Required, DisplayName("Email"), EmailAddress(ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }
        [Required, DisplayName("Password"), MinLength(6, ErrorMessage = "Password must contain at least 6 characters.")]
        public string Password { get; set; }
        [Required, DisplayName("Confirm password"), Compare("Password", ErrorMessage = "Passwords don't match.")]
        public string RePassword { get; set; }
        [Required, DisplayName("I am a manager.")]
        public bool IsManager { get; set; }
    }
}