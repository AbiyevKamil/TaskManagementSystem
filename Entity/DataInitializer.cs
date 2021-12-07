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
                FullName = "Kamil Abiyev",
                Password = Crypto.HashPassword("123456"),
                RegisteredDate = DateTime.Now,
                Username = "kamil"
            };
            context.Managers.Add(m);
            context.SaveChanges();

            Worker w = new Worker()
            {
                Email = "kamilabiyev1903@gmail.com",
                FullName = "Kamil Abiyev Worker",
                Password = Crypto.HashPassword("123456"),
                RegisteredDate = DateTime.Now,
                Username = "kamil_worker"
            };

            context.Workers.Add(w);
            context.SaveChanges();

            Task t = new Task()
            {
                Title = "Test",
                Description = "Hey, this is description of task.Hey, this is description of task.Hey, this is description of task.Hey, this is description of task.Hey, this is description of task.Hey, this is description of task.",
                IsCompleted = false,
                IsMissing = false,
                ManagerId = m.Id,
                WorkerId = w.Id,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(15),
                IsPublic = true,
                AddedDate = DateTime.Now,
            };
            context.Tasks.Add(t);
            Task t1 = new Task()
            {
                Title = "Test2",
                Description = "Hey, this is description of task.Hey, this is description of task.Hey, this is description of task.Hey, this is description of task.Hey, this is description of task.Hey, this is description of task.",
                IsCompleted = false,
                IsMissing = false,
                ManagerId = m.Id,
                WorkerId = w.Id,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                IsPublic = true,
                AddedDate = DateTime.Now,
            };
            context.Tasks.Add(t1);
            Task t2 = new Task()
            {
                Title = "Test3",
                Description = "Hey, this is description of task.Hey, this is description of task.Hey, this is description of task.Hey, this is description of task.Hey, this is description of task.Hey, this is description of task.",
                IsCompleted = false,
                IsMissing = false,
                ManagerId = m.Id,
                WorkerId = w.Id,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(6),
                IsPublic = true,
                AddedDate = DateTime.Now,
            };
            context.Tasks.Add(t2);
            Task t3 = new Task()
            {
                Title = "Test4",
                Description = "Hey, this is description of task.Hey, this is description of task.Hey, this is description of task.Hey, this is description of task.Hey, this is description of task.Hey, this is description of task.",
                IsCompleted = false,
                IsMissing = false,
                ManagerId = m.Id,
                WorkerId = w.Id,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                IsPublic = true,
                AddedDate = DateTime.Now,
            };
            context.Tasks.Add(t3);
            context.SaveChanges();

            base.Seed(context);
        }
    }
}