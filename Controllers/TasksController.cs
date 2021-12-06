using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManagementSystem.Entity;

namespace TaskManagementSystem.Controllers
{
    public class TasksController : Controller
    {
        private DataContext context = new DataContext();
        public ActionResult Index()
        {
            return View();
        }
    }
}