using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using TaskManagementSystem.Entity;
using TaskManagementSystem.Helpers;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private DataContext context = new DataContext();
        private readonly string generalEmail = "stphoenix2002@gmail.com";
        private readonly string generalPass = "akamil2002";
        private readonly string companyName = "Task Management Company";

        [HttpGet]
        public ActionResult Register()
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
                ModelState.AddModelError("", "Username or model.Email is already registered");
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


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var oldManagerUser = context.Managers.FirstOrDefault(i => i.Email == model.Email);
                if (oldManagerUser != null)
                {
                    if (Crypto.VerifyHashedPassword(oldManagerUser.Password, model.Password))
                    {
                        var cValue = Hasher.Encrypt(oldManagerUser.Id.ToString());
                        Response.Cookies["AuthToken"].Value = cValue;
                        Response.Cookies["AuthToken"].Expires =
                            model.RememberMe ? DateTime.Now.AddDays(3) : DateTime.Now.AddHours(1);
                        return RedirectToAction("Index", "Home");
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
                            Hasher.Encrypt(oldWorkerUser.Id.ToString());
                        Response.Cookies["AuthToken"].Value = cValue;
                        Response.Cookies["AuthToken"].Expires =
                            model.RememberMe ? DateTime.Now.AddDays(3) : DateTime.Now.AddHours(1);
                        return RedirectToAction("Index", "Home");
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

        [HttpGet]
        public ActionResult ForgotPassword()
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

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPassEmail model)
        {
            if (ModelState.IsValid)
            {
                var oldWorkerEmail = context.Workers.FirstOrDefault(i => i.Email == model.Email);
                if (oldWorkerEmail != null)
                {
                    SendEmail(model.Email);
                    return RedirectToAction("CheckRecoveryCode", new
                    {
                        email = Hasher.EncryptEmail(model.Email)
                    });
                }
                var oldManagerEmail = context.Managers.FirstOrDefault(i => i.Email == model.Email);
                if (oldManagerEmail != null)
                {
                    SendEmail(model.Email);
                    return RedirectToAction("CheckRecoveryCode", new
                    {
                        email = Hasher.EncryptEmail(model.Email)
                    });
                }
                ModelState.AddModelError("", "Email is not registered");
            }
            return View(model);
        }

        [HttpGet, Route("Account/CheckRecoveryCode/{email}")]
        public ActionResult CheckRecoveryCode(string email)
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

        [HttpPost, Route("Account/CheckRecoveryCode/{email}")]
        public ActionResult CheckRecoveryCode(CheckRecoveryCodeModel model, string email)
        {
            try
            {
                email = Hasher.DecryptEmail(email);
            }
            catch
            {
                return RedirectToAction("Login");
            }
            DeleteOldData();
            if (ModelState.IsValid)
            {
                var userDataFromEmail = context.RecoveryCodes.FirstOrDefault(i => i.UserEmail == email);
                if (userDataFromEmail != null)
                {
                    if (model.Code == userDataFromEmail.Code)
                    {
                        email = Hasher.EncryptEmail(model.Email);
                        return RedirectToAction("ResetPassword", new
                        {
                            email
                        });
                    }
                }
                ModelState.AddModelError("", "Code is not valid.");
            }
            return View(model);
        }

        [HttpGet, Route("Account/ResetPassword/{email}")]
        public ActionResult ResetPassword(string email)
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

        [HttpPost, Route("Account/ResetPassword/{email}")]
        public ActionResult ResetPassword(ResetModel model, string email)
        {
            try
            {
                email = Hasher.DecryptEmail(email);
                email = Hasher.DecryptEmail(email);
            }
            catch
            {
                return RedirectToAction("Login");
            }
            if (ModelState.IsValid)
            {
                var userManager = context.Managers.FirstOrDefault(i => i.Email == email);
                if (userManager != null)
                {
                    userManager.Password = Crypto.HashPassword(model.Password);
                    context.SaveChanges();
                    return RedirectToAction("Login");
                }
                var userWorker = context.Workers.FirstOrDefault(i => i.Email == email);
                if (userWorker != null)
                {
                    userWorker.Password = Crypto.HashPassword(model.Password);
                    context.SaveChanges();
                    return RedirectToAction("Login");
                }
            }
            return View(model);
        }

        private void DeleteOldData()
        {
            var dataMustBeDeleted = context.RecoveryCodes.Where(i => i.DeleteTime < DateTime.Now).ToList();
            foreach (var data in dataMustBeDeleted)
            {
                context.RecoveryCodes.Remove(data);
            }

            context.SaveChanges();
        }

        private void DeleteOldDataByEmail(string email)
        {
            var dataMustBeDeleted = context.RecoveryCodes.Where(i => i.UserEmail == email).ToList();
            foreach (var data in dataMustBeDeleted)
            {
                context.RecoveryCodes.Remove(data);
            }

            context.SaveChanges();
        }

        private int GetRandomCode()
        {
            Random r = new Random();
            return r.Next(10000, 99999);
        }

        private void SendEmail(string email)
        {
            string recoveryCode = (GetRandomCode()).ToString();
            string emailBody = $"Your reset code: <b>{recoveryCode}</b>";
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(generalEmail);
            message.To.Add(new MailAddress(email));
            message.Subject = $"Password Recovery | {companyName}";
            message.IsBodyHtml = true;
            message.Body = emailBody;
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(generalEmail, generalPass);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
            DeleteOldDataByEmail(email);
            RecoveryCode rc = new RecoveryCode()
            {
                Code = recoveryCode,
                UserEmail = email,
                DeleteTime = DateTime.Now.AddMinutes(15),
            };
            context.RecoveryCodes.Add(rc);
            context.SaveChanges();
        }
    }
}