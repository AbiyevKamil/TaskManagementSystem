using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TaskManagementSystem.Models
{
    public class CheckRecoveryCodeModel
    {
        [Required]
        public string Code { get; set; }
        public string Email { get; set; }
    }
}