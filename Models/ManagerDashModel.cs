using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManagementSystem.Entity;

namespace TaskManagementSystem.Models
{
    public class ManagerDashModel
    {
        public Manager Manager { get; set; }
        public List<Task> Tasks { get; set; }
    }
}