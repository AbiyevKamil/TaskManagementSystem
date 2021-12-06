using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManagementSystem.Models
{
    public class LoginModel
    {
        [Required, DisplayName("Email"), EmailAddress(ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }
        [Required, DisplayName("Password"), MinLength(6, ErrorMessage = "Password must contain at least 6 characters.")]
        public string Password { get; set; }
        [Required, DisplayName("Remember me")]
        public bool RememberMe { get; set; }
    }
}