using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManagementSystem.Entity;

namespace TaskManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private DataContext context = new DataContext();
        public ActionResult Index()
        {
            var tasks = context.Tasks.Include(i => i.Worker).Include(i => i.Manager).Where(i => i.IsPublic).ToList().Select(i => new Task()
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                IsCompleted = i.IsCompleted,
                IsPublic = i.IsPublic,
                Manager = i.Manager,
                Worker = i.Worker,
                StartDate = i.StartDate,
                EndDate = i.EndDate,
                IsMissing = i.EndDate < DateTime.Now ? true : false
            });
            return View(tasks);
        }

        [Route("Home/TaskDetail/{id}")]
        public ActionResult TaskDetail(int Id)
        {
            var task = context.Tasks.Include(i => i.Manager).Include(i => i.Worker).FirstOrDefault(i => i.Id == Id);
            if (task != null)
            {
                if (task.IsPublic)
                    return View(task);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}