using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaskManagementSystem.Entity;
using TaskManagementSystem.Helpers;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    public class ManagerController : Controller
    {
        private DataContext context = new DataContext();
        public ActionResult Dashboard()
        {
            if (Request.Cookies["AuthToken"] != null)
            {
                var cValue = Request.Cookies["AuthToken"].Value;
                if (!String.IsNullOrEmpty(cValue))
                {
                    var cookie = Hasher.Decrypt(cValue);
                    int Id = Convert.ToInt32(cookie);
                    var manager = context.Managers.FirstOrDefault(i => i.Id == Id);
                    if (manager != null)
                    {
                        var tasks = context.Tasks.Include(i => i.Manager).Include(i => i.Worker).Where(i => i.ManagerId == manager.Id)
                            .ToList().Select(i => new Task()
                            {
                                Id = i.Id,
                                Title = i.Title,
                                Description = i.Description.Length > 40 ? $"{i.Description.Substring(0, 37)}..." : i.Description,
                                IsCompleted = i.IsCompleted,
                                IsPublic = i.IsPublic,
                                IsMissing = i.IsMissing,
                                Manager = i.Manager,
                                ManagerId = i.ManagerId,
                                EndDate = i.EndDate,
                                StartDate = i.StartDate,
                                Worker = i.Worker,
                                WorkerId = i.WorkerId,
                            }).ToList();
                        var data = new ManagerDashModel()
                        {
                            Manager = manager,
                            Tasks = tasks
                        };
                        return View(data);
                    }
                    else
                    {
                        var worker = context.Workers.FirstOrDefault(i => i.Id == Id);
                        if (worker != null)
                        {
                            // Redirect to Worker Dashboard
                            //return RedirectToAction("Dashboard", "Worker");
                        }
                    }
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult AddTask()
        {
            ViewBag.WorkerId = new SelectList(context.Workers, "Id", "Username");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddTask(AddTaskModel model)
        {
            if (ModelState.IsValid)
            {
                if (!(DateTime.Now > model.EndDate))
                {
                    if (Request.Cookies["AuthToken"] != null)
                    {
                        var cValue = Request.Cookies["AuthToken"].Value;
                        if (!String.IsNullOrEmpty(cValue))
                        {
                            var cookie = Hasher.Decrypt(cValue);
                            int Id = Convert.ToInt32(cookie);
                            var manager = context.Managers.FirstOrDefault(i => i.Id == Id);
                            if (manager != null)
                            {
                                var task = new Task()
                                {
                                    Title = model.Title,
                                    Description = model.Description,
                                    IsPublic = model.IsPublic,
                                    StartDate = DateTime.Now,
                                    EndDate = model.EndDate,
                                    WorkerId = model.WorkerId,
                                    IsMissing = false,
                                    ManagerId = manager.Id
                                };
                                context.Tasks.Add(task);
                                context.SaveChanges();
                                return RedirectToAction("Dashboard");
                            }
                        }
                    }
                    return RedirectToAction("Login", "Account");
                }
                ModelState.AddModelError("", "End date is not valid.");
            }
            ViewBag.WorkerId = new SelectList(context.Workers, "Id", "Username", model.WorkerId);
            return View(model);
        }

        [HttpGet, Route("Manager/MyTaskDetail/{id}")]
        public ActionResult MyTaskDetail(int id)
        {
            if (Request.Cookies["AuthToken"] != null)
            {
                var cValue = Request.Cookies["AuthToken"].Value;
                if (!String.IsNullOrEmpty(cValue))
                {
                    var cookie = Hasher.Decrypt(cValue);
                    int Id = Convert.ToInt32(cookie);
                    var manager = context.Managers.FirstOrDefault(i => i.Id == Id);
                    if (manager != null)
                    {
                        var task = context.Tasks.Include(i => i.Manager).Include(i => i.Worker)
                            .FirstOrDefault(i => i.Id == id);
                        var data = new ManagerDetailModel()
                        {
                            Manager = manager,
                            Task = task
                        };
                        return View(data);
                    }
                }
            }
            return RedirectToAction("Login", "Account");
        }

    }
}