using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManagementSystem.Entity;

namespace TaskManagementSystem.Models
{
    public class WorkerDetailModel
    {
        public Worker Worker { get; set; }
        public Task Task { get; set; }
    }
}