
#region Using Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DataModel.Modal;
using CustomAuthentication;
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
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserDetail> UsersDetail { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Convention> Conventions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<VisitType> VisitTypes { get; set; }
        public DbSet<Audience> Audiences { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<FileResource> FileResources { get; set; }
        public DbSet<ConventionAttachment> ConventionAttachments { get; set; }
    }
}