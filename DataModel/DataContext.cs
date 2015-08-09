
#region Using Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DataModel.Modal;
#endregion

namespace DataModel
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection")
        {
        }
        public DataContext(string connectionName) : base(connectionName)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            OfficeMananer.OnModelCreating(modelBuilder);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<Task> Tasks { get; set; }
    }
}