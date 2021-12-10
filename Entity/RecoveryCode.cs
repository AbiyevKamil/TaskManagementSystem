using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManagementSystem.Entity
{
    public class RecoveryCode
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public DateTime DeleteTime { get; set; }
    }
}