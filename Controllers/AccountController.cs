using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
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
                        if (model.RememberMe)
                        {
                            Session.Timeout = 1440;
                            Session["AuthToken"] = oldManagerUser.Id;
                        }
                        else
                        {
                            Session.Timeout = 60;
                            Session["AuthToken"] = oldManagerUser.Id;
                        }
                        return RedirectToAction("Index", "Home");
                        //if (String.IsNullOrEmpty(returnUrl))
                        //    return RedirectToAction("Index", "Tasks");
                        //return RedirectToAction("Dashboard", "Manager");
                    }
                }
                var oldWorkerUser = context.Workers.FirstOrDefault(i => i.Email == model.Email);
                if (oldWorkerUser != null)
                {
                    if (Crypto.VerifyHashedPassword(oldWorkerUser.Password, model.Password))
                    {
                        HttpCookie StudentCookies = new HttpCookie("StudentCookies")
                        {
                            Value = oldWorkerUser.Id.ToString(),
                            Expires = model.RememberMe ? DateTime.Now.AddDays(3) : DateTime.Now.AddHours(1)
                        };
                        Response.Cookies.Add(StudentCookies);
                        Response.Flush();
                        //if (model.RememberMe)
                        //{
                        //    Session.Timeout = 1440;
                        //    Session["AuthToken"] = oldWorkerUser.Id;
                        //}
                        //else
                        //{
                        //    Session.Timeout = 60;
                        //    Session["AuthToken"] = oldWorkerUser.Id;
                        //}
                        if (String.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index", "Home");
                        //return RedirectToAction("Dashboard", "Worker");
                    }
                }
                ModelState.AddModelError("", "Email is not registered.");
            }
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Remove("AuthToken");
            return RedirectToAction("Index", "Home");
        }
    }
}