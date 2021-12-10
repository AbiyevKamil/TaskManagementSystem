using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaskManagementSystem.Entity;
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
                    var cookie = Encoding.UTF8.GetString(MachineKey.Unprotect(Convert.FromBase64String(cValue)));
                    int Id = Convert.ToInt32(cookie.ToString());
                    var manager = context.Managers.FirstOrDefault(i => i.Id == Id);
                    if (manager != null)
                    {
                        var tasks = context.Tasks.Include(i => i.Manager).Include(i => i.Worker).Where(i => i.ManagerId == manager.Id)
                            .ToList().Select(i => new Task()
                            {
                                Id = i.Id,
                                Title = i.Title,
                                Description = i.Description.Length > 40 ? $"{i.Description.Substring(0, 37)}..." : i.Description,
                                AddedDate = i.AddedDate,
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

                        }
                    }
                }
            }
            return RedirectToAction("Login", "Account");
        }
    }
}