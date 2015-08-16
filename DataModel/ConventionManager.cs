﻿using DataModel.Modal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class ConventionManager
    {
        private DataContext Context = new DataContext();
        public static void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Modal.UserDetail>()
                .HasMany(u => u.Conventions)
                .WithMany(r => r.UsersDetail)
                .Map(model =>
                {
                    model.ToTable("UserConventions");
                    model.MapLeftKey("UserId");
                    model.MapRightKey("ConventionId");
                });
        }
        public bool Add(string name, DateTime startDate, DateTime endDate, string description, int userID)
        {
            try
            {
                var users = Context.UsersDetail.Where(model => model.UserId == userID).ToList();
                if (users == null || users.Count <= 0) { return false; }
                Context.Conventions.Add(new Modal.Convention
                {
                    Name = name,
                    StartDate = startDate,
                    EndDate = endDate,
                    Description = description,
                    UsersDetail = users,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                });
                var status = Context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<Convention> GetConventions()
        { return Context.Conventions.Where(model => model.IsDeleted == false).ToList(); }
        public Convention GetConventionDetail(int id)
        { return Context.Conventions.Where(modal => modal.ConventionId == id).FirstOrDefault(); }
        public bool Update(string name, DateTime startDate, DateTime endDate, string description, int userID, int conventionID)
        {
            try
            {
                var users = Context.UsersDetail.Where(model => model.UserId == userID).ToList();
                var conventionDetail = Context.Conventions.Where(model => model.ConventionId == conventionID).FirstOrDefault();
                if (conventionDetail == null) return false;
                conventionDetail.Name = name;
                conventionDetail.StartDate = startDate;
                conventionDetail.EndDate = endDate;
                conventionDetail.Description = description;
                conventionDetail.UpdateDate = DateTime.Now;
                conventionDetail.UsersDetail = users;
                Context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete(int id)
        {
            var conventionDetail = Context.Conventions.Where(model => model.ConventionId == id).FirstOrDefault();
            if (conventionDetail == null) { return false; }
            conventionDetail.IsDeleted = true;
            Context.SaveChanges();
            return true;
        }
    }
}