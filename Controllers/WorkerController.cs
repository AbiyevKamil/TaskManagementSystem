using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManagementSystem.Entity;
using TaskManagementSystem.Helpers;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    public class WorkerController : Controller
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
                    var worker = context.Workers.FirstOrDefault(i => i.Id == Id);
                    if (worker != null)
                    {
                        var tasks = context.Tasks.Include(i => i.Worker).Include(i => i.Worker).Where(i => i.WorkerId == worker.Id)
                            .ToList().Select(i => new Task()
                            {
                                Id = i.Id,
                                Title = i.Title,
                                Description = i.Description.Length > 40 ? $"{i.Description.Substring(0, 37)}..." : i.Description,
                                IsCompleted = i.IsCompleted,
                                IsPublic = i.IsPublic,
                                IsMissing = i.IsCompleted ? false : i.EndDate < DateTime.Now ? true : false,
                                Manager = i.Manager,
                                ManagerId = i.ManagerId,
                                EndDate = i.EndDate,
                                StartDate = i.StartDate,
                                Worker = i.Worker,
                                WorkerId = i.WorkerId,
                            }).ToList();
                        var data = new WorkerDashModel()
                        {
                            Worker = worker,
                            Tasks = tasks
                        };
                        return View(data);
                    }
                    else
                    {
                        var manager = context.Managers.FirstOrDefault(i => i.Id == Id);
                        if (manager != null)
                        {
                            // Redirect to Worker Dashboard
                            return RedirectToAction("Dashboard", "Manager");
                        }
                    }
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet, Route("Manager/MyTaskDetail/{id}")]
        public ActionResult MyTaskDetail(int? id)
        {
            if (id == null)
                return RedirectToAction("Dashboard");
            if (Request.Cookies["AuthToken"] != null)
            {
                var cValue = Request.Cookies["AuthToken"].Value;
                if (!String.IsNullOrEmpty(cValue))
                {
                    var cookie = Hasher.Decrypt(cValue);
                    int Id = Convert.ToInt32(cookie);
                    var worker = context.Workers.FirstOrDefault(i => i.Id == Id);
                    if (worker != null)
                    {
                        var task = context.Tasks.Include(i => i.Manager).Include(i => i.Worker)
                            .FirstOrDefault(i => i.Id == id && i.WorkerId == worker.Id);
                        if (task != null)
                        {
                            var data = new WorkerDetailModel()
                            {
                                Worker = worker,
                                Task = task
                            };
                            return View(data);
                        }

                        return RedirectToAction("Dashboard");
                    }
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet, Route("Worker/EditMyTask/{id}")]
        public ActionResult EditMyTask(int? id)
        {
            if (id == null)
                return RedirectToAction("Dashboard");
            if (Request.Cookies["AuthToken"] != null)
            {
                var cValue = Request.Cookies["AuthToken"].Value;
                if (!String.IsNullOrEmpty(cValue))
                {
                    var cookie = Hasher.Decrypt(cValue);
                    int Id = Convert.ToInt32(cookie);
                    var worker = context.Workers.FirstOrDefault(i => i.Id == Id);
                    if (worker != null)
                    {
                        var task = context.Tasks.Include(i => i.Manager).Include(i => i.Worker)
                            .FirstOrDefault(i => i.Id == id && i.WorkerId == worker.Id);
                        if (task != null)
                        {
                            ViewBag.User = worker;
                            ViewBag.Task = task;
                            var model = new WorkerTaskUpdateModel()
                            {
                                Id = task.Id,
                                IsCompleted = task.IsCompleted
                            };
                            return View(model);
                        }

                        return RedirectToAction("Dashboard");
                    }
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost, Route("Worker/EditMyTask/{id}")]
        public ActionResult EditMyTask(WorkerTaskUpdateModel model, int id)
        {
            if (Request.Cookies["AuthToken"] != null)
            {
                var cValue = Request.Cookies["AuthToken"].Value;
                if (!String.IsNullOrEmpty(cValue))
                {
                    var cookie = Hasher.Decrypt(cValue);
                    int Id = Convert.ToInt32(cookie);
                    var worker = context.Workers.FirstOrDefault(i => i.Id == Id);
                    if (worker != null)
                    {
                        if (ModelState.IsValid)
                        {
                            var task = context.Tasks.Include(i => i.Manager).Include(i => i.Worker).FirstOrDefault(i => i.Id == id);
                            if (task != null)
                            {
                                task.IsCompleted = model.IsCompleted;
                                context.SaveChanges();
                                return RedirectToAction("Dashboard");
                            }
                        }
                        return View(model);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Login", "Account");
        }

    }
}