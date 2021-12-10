using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaskManagementSystem.Entity;

namespace TaskManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private DataContext context = new DataContext();
        public ActionResult Index()
        {
            if (Request.Cookies["AuthToken"] != null)
            {
                var cValue = Request.Cookies["AuthToken"].Value;
                if (!String.IsNullOrEmpty(cValue))
                {
                    var cookie = Encoding.UTF8.GetString(MachineKey.Unprotect(Convert.FromBase64String(cValue)));
                    int Id = Convert.ToInt32(cookie.ToString());
                    var manager = context.Managers.FirstOrDefault(i => i.Id == Id);
                    if (manager != null)
                    {
                        ViewBag.User = manager;
                    }
                    else
                    {
                        var worker = context.Workers.FirstOrDefault(i => i.Id == Id);
                        if (worker != null)
                        {
                            ViewBag.User = worker;
                        }
                    }
                }
            }
            var tasks = context.Tasks.Include(i => i.Worker).Include(i => i.Manager).Where(i => i.IsPublic).OrderByDescending(i => i.AddedDate).ToList().Select(i => new Task()
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