using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManagementSystem.Entity
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsPublic { get; set; }
        public bool IsMissing { get; set; }

        public int WorkerId { get; set; }
        public int ManagerId { get; set; }

        public virtual Worker Worker { get; set; }
        public virtual Manager Manager { get; set; }
    }
}