using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TaskManagementSystem.Entity
{
    public class DataContext : DbContext
    {
        public DataContext() : base("TasksDB")
        {
            Database.SetInitializer(new DataInitializer());
        }

        public DbSet<Task> Tasks { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<RecoveryCode> RecoveryCodes { get; set; }

    }
}