using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManagementSystem.Models
{
    public class ForgotPassEmail
    {
        [Required, DisplayName("Email"), EmailAddress(ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }
    }
}