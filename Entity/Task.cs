using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManagementSystem.Entity
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        [Required, DisplayName("Title")]
        public string Title { get; set; }
        [Required, DisplayName("Description")]
        public string Description { get; set; }
        [Required, DataType(DataType.Date), DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
        [Required, DataType(DataType.Date), DisplayName("End Date")]
        public DateTime EndDate { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required, DisplayName("Is Completed")]
        public bool IsCompleted { get; set; }
        [Required, DisplayName("Is Public")]
        public bool IsPublic { get; set; }
        public bool IsMissing { get; set; }

        public int WorkerId { get; set; }
        public int ManagerId { get; set; }

        public virtual Worker Worker { get; set; }
        public virtual Manager Manager { get; set; }
    }
}