using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using TaskManagementSystem.Entity;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private DataContext context = new DataContext();

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var oldManager = context.Managers.FirstOrDefault(i => i.Username == model.Username || i.Email == model.Email);
                if (oldManager == null)
                {
                    var oldWorker = context.Workers.FirstOrDefault(i => i.Username == model.Username || i.Email == model.Email);
                    if (oldWorker == null)
                    {
                        if (model.IsManager)
                        {
                            Manager user = new Manager()
                            {
                                FullName = model.FullName,
                                Username = model.Username,
                                Email = model.Email,
                                Password = Crypto.HashPassword(model.Password),
                                RegisteredDate = DateTime.Now,
                            };
                            context.Managers.Add(user);
                            context.SaveChanges();
                        }
                        else
                        {
                            Worker user = new Worker()
                            {
                                FullName = model.FullName,
                                Username = model.Username,
                                Email = model.Email,
                                Password = Crypto.HashPassword(model.Password),
                                RegisteredDate = DateTime.Now,
                            };
                            context.Workers.Add(user);
                            context.SaveChanges();
                        }

                        return RedirectToAction("Login");
                    }
                }
                ModelState.AddModelError("", "Username or email is already registered");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
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
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        var worker = context.Workers.FirstOrDefault(i => i.Id == Id);
                        if (worker != null)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            return View();
        }

        // TAKE A LOOK HERE

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            string returnUrl = Request.Params["returnUrl"];
            if (ModelState.IsValid)
            {
                var oldManagerUser = context.Managers.FirstOrDefault(i => i.Email == model.Email);
                if (oldManagerUser != null)
                {
                    if (Crypto.VerifyHashedPassword(oldManagerUser.Password, model.Password))
                    {
                        var cValue =
                            Convert.ToBase64String(
                                MachineKey.Protect(Encoding.UTF8.GetBytes(oldManagerUser.Id.ToString())));
                        Response.Cookies["AuthToken"].Value = cValue;
                        Response.Cookies["AuthToken"].Expires =
                            model.RememberMe ? DateTime.Now.AddDays(3) : DateTime.Now.AddHours(1);
                        return RedirectToAction("Index", "Home");
                        //if (String.IsNullOrEmpty(returnUrl))
                        //    return RedirectToAction("Index", "Tasks");
                        //return RedirectToAction("Dashboard", "Manager");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Wrong password");
                        return View(model);
                    }
                }
                var oldWorkerUser = context.Workers.FirstOrDefault(i => i.Email == model.Email);
                if (oldWorkerUser != null)
                {
                    if (Crypto.VerifyHashedPassword(oldWorkerUser.Password, model.Password))
                    {
                        var cValue =
                            Convert.ToBase64String(
                                MachineKey.Protect(Encoding.UTF8.GetBytes(oldWorkerUser.Id.ToString())));
                        Response.Cookies["AuthToken"].Value = cValue;
                        Response.Cookies["AuthToken"].Expires =
                            model.RememberMe ? DateTime.Now.AddDays(3) : DateTime.Now.AddHours(1);
                        if (String.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index", "Home");
                        //return RedirectToAction("Dashboard", "Worker");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Wrong password");
                        return View(model);
                    }
                }
                ModelState.AddModelError("", "Email is not registered.");
            }
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            Response.Cookies["AuthToken"].Value = "";
            return RedirectToAction("Index", "Home");
        }
    }
}