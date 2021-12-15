using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManagementSystem.Entity
{
    public class Manager
    {
        [Key]
        public int Id { get; set; }
        [Required, DisplayName("Fullname")]
        public string FullName { get; set; }
        [Required, DisplayName("Username")]
        public string Username { get; set; }
        [Required, DisplayName("Email"), EmailAddress]
        public string Email { get; set; }
        [Required, DisplayName("Password")]
        public string Password { get; set; }
        [Required]
        public DateTime RegisteredDate { get; set; }
        [DefaultValue(true)]
        public bool IsManager { get; set; }

        public ICollection<Task> Tasks { get; set; }
    }
}