using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace TaskManagementSystem.Entity
{
    public class DataInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            Manager m = new Manager()
            {
                Email = "kabiyev@std.beu.edu.az",
                Fullname = "Kamil Abiyev",
                Password = Crypto.HashPassword("123456"),
                RegisteredDate = DateTime.Now,
                Username = "kamil"
            };
            context.Managers.Add(m);
            context.SaveChanges();

            Worker w = new Worker()
            {
                Email = "kamilabiyev1903@gmail.com",
                Fullname = "Kamil Abiyev Worker",
                Password = Crypto.HashPassword("123456"),
                RegisteredDate = DateTime.Now,
                Username = "kamil_worker"
            };

            context.Workers.Add(w);
            context.SaveChanges();

            DateTime dt1 = DateTime.Now;
            DateTime dt2 = dt1.AddDays(15);
            Task t = new Task()
            {
                Title = "Test",
                Description = "Hey, this is description of task.Hey, this is description of task.Hey, this is description of task.Hey, this is description of task.Hey, this is description of task.Hey, this is description of task.",
                IsCompleted = false,
                IsMissing = false,
                ManagerId = m.Id,
                WorkerId = w.Id,
                StartDate = dt1,
                EndDate = dt2,
                IsPublic = true
            };
            context.Tasks.Add(t);
            context.SaveChanges();

            base.Seed(context);
        }
    }
}