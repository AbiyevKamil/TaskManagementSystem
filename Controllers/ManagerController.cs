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
                                IsMissing = i.IsCompleted ? false : i.EndDate < DateTime.Now ? true : false,
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
                        ViewBag.WorkerId = new SelectList(context.Workers, "Id", "Username");
                        return View();
                    }
                }
            }

            return RedirectToAction("Login", "Account");
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
                    var manager = context.Managers.FirstOrDefault(i => i.Id == Id);
                    if (manager != null)
                    {
                        var task = context.Tasks.Include(i => i.Manager).Include(i => i.Worker)
                            .FirstOrDefault(i => i.Id == id && i.Manager.Id == manager.Id);
                        if (task != null)
                        {
                            var data = new ManagerDetailModel()
                            {
                                Manager = manager,
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

        [HttpGet, Route("Manager/EditMyTask/{id}")]
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
                    var manager = context.Managers.FirstOrDefault(i => i.Id == Id);
                    if (manager != null)
                    {
                        var task = context.Tasks.Include(i => i.Manager).Include(i => i.Worker)
                            .FirstOrDefault(i => i.Id == id && i.Manager.Id == manager.Id);
                        if (task != null)
                        {
                            ViewBag.User = manager;
                            var model = new TaskUpdateModel()
                            {
                                Id = task.Id,
                                Title = task.Title,
                                Description = task.Description,
                                StartDate = task.StartDate,
                                EndDate = task.EndDate,
                                IsCompleted = task.IsCompleted,
                                IsPublic = task.IsPublic,
                                WorkerId = task.WorkerId,
                            };
                            ViewBag.WorkerId = new SelectList(context.Workers, "Id", "Username", model.WorkerId);
                            return View(model);
                        }

                        return RedirectToAction("Dashboard");
                    }
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost, ValidateAntiForgeryToken, Route("Manager/EditMyTask/{id}")]
        public ActionResult EditMyTask(TaskUpdateModel model, int id)
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
                        if (ModelState.IsValid)
                        {
                            var task = context.Tasks.Include(i => i.Manager).Include(i => i.Worker).FirstOrDefault(i => i.Id == id);
                            if (task != null)
                            {
                                task.StartDate = model.StartDate;
                                task.EndDate = model.EndDate;
                                task.Title = model.Title;
                                task.Description = model.Description;
                                task.WorkerId = model.WorkerId;
                                task.IsCompleted = model.IsCompleted;
                                task.IsPublic = model.IsPublic;
                                context.SaveChanges();
                                return RedirectToAction("Dashboard");
                            }
                        }
                        ViewBag.WorkerId = new SelectList(context.Workers, "Id", "Username", model.WorkerId);
                        return View(model);
                    }
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet, Route("Manager/DeleteMyTask/{id}")]
        public ActionResult DeleteMyTask(int? id)
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
                    var manager = context.Managers.FirstOrDefault(i => i.Id == Id);
                    if (manager != null)
                    {
                        var task = context.Tasks.Include(i => i.Manager).Include(i => i.Worker)
                            .FirstOrDefault(i => i.Id == id && i.Manager.Id == manager.Id);
                        if (task != null)
                        {
                            ViewBag.User = manager;
                            return View(task);
                        }

                        return RedirectToAction("Dashboard");
                    }
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost, Route("Manager/DeleteMyTaskConfirmed/{id}")]
        public ActionResult DeleteMyTaskConfirmed(int id)
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
                            .FirstOrDefault(i => i.Id == id && i.Manager.Id == manager.Id);
                        if (task != null)
                        {
                            context.Tasks.Remove(task);
                            context.SaveChanges();
                        }

                        return RedirectToAction("Dashboard");
                    }
                }
            }
            return RedirectToAction("Login", "Account");
        }
    }
}