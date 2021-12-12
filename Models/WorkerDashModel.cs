using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManagementSystem.Entity;

namespace TaskManagementSystem.Models
{
    public class WorkerDashModel
    {
        public Worker Worker { get; set; }
        public List<Task> Tasks { get; set; }
    }
}