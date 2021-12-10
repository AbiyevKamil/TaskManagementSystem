using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManagementSystem.Models
{
    public class ResetModel
    {
        [Required, DisplayName("New password"), MinLength(6, ErrorMessage = "Password must contain at least 6 characters.")]
        public string Password { get; set; }
        [Required, DisplayName("Confirm new password"), Compare("Password", ErrorMessage = "Passwords don't match.")]
        public string RePassword { get; set; }
    }
}