using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePlatform.Controllers
{
    public class ConventionsController : BaseController
    {
        public object GetConventionBookingTarget(int id)
        {
            var conventionManager = new ConventionManager();
            var convention = conventionManager.GetConventionDetail(id);
            var totalAchievedTargets = convention.Audiences.Where(model => model.IsBooked == true && model.IsDeleted == false).ToList().Count();
            return new { Total = 0, ActTotal = totalAchievedTargets };
        }

        public object GetFundRaisingTarget(int id)
        {
            var conventionManager = new ConventionManager();
            var convention = conventionManager.GetConventionDetail(id);
            var totalAchievedTargets = convention.Audiences.Where(model => model.IsBooked == true && model.IsDeleted == false).Sum(model => model.Amount);
            return new { Total = 0, ActTotal = totalAchievedTargets };
        }

        public object GetGSBAountTarget(int id)
        {
            var conventionManager = new ConventionManager();
            var convention = conventionManager.GetConventionDetail(id);
            var totalAchievedTargets = convention.Audiences.Where(model => model.IsBooked == true && model.IsDeleted == false).Sum(model => model.GSBAmount);
            return new { Total = 0, ActTotal = totalAchievedTargets };
        }

        public object GetEventsTarget(int id)
        {
            var conventionManager = new ConventionManager();
            var convention = conventionManager.GetConventionDetail(id);
            var totalAchievedTargets = convention.Events.Where(model => model.IsDeleted == false && model.EndDate <= DateTime.Now).ToList().Count();
            return new { Total = 0, ActTotal = totalAchievedTargets };
        }

        public ActionResult Index()
        {
            BundleConfig.AddStyle("/Conventions", "Conventions.css", ControllerName);
            BundleConfig.AddScript("~/Scripts/Conventions", "Convention.js", ControllerName);

            return View();
        }

        public ActionResult Add()
        {
            var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
            //var roleID = customRoleProvider.GetRole("Speakers");
            var customMembershipProvider = new CustomAuthentication.CustomMembershipProvider();
            //var Users = customMembershipProvider.GetUsers(roleID.RoleId);
            return PartialView();
        }

        [HttpPost]
        public JsonResult Add(string name, DateTime startDate, DateTime endDate, string description, int userId, string city)
        {
            var conventionManager = new ConventionManager();
            return Json(conventionManager.Add(name, startDate, endDate, description, userId, city));
        }

        public JsonResult GetConventions(int pageNo = 1, int pageSize = 3)
        {
            var conventionManager = new ConventionManager();
            var conventions = conventionManager.GetConventions();
            var totalRecord = conventions.Count();
            var filteredConventions = conventions.Select(modal => new
            {
                id = modal.ConventionId,
                Name = modal.Name,
                StartDate = modal.StartDate.ToString("dd-MM-yyyy HH:mm"),
                EndDate = modal.EndDate.ToString("dd-MM-yyyy HH:mm"),
                Booking = GetConventionBookingTarget(modal.ConventionId),
                Donation = GetFundRaisingTarget(modal.ConventionId),
                GSBAmount = GetGSBAountTarget(modal.ConventionId),
                Events = GetEventsTarget(modal.ConventionId)
            }).OrderBy(modal => modal.StartDate).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return Json(new
            {
                totalRecord = totalRecord,
                currentPage = pageNo,
                pageSize = pageSize,
                conventions = filteredConventions
            });
        }
        public PartialViewResult Edit(int id)
        {
            var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
            var roleID = customRoleProvider.GetRole("Speakers");
            var customMembershipProvider = new CustomAuthentication.CustomMembershipProvider();
            var conventionManager = new ConventionManager();
            var conventionDetail = conventionManager.GetConventionDetail(id);
            return PartialView(conventionDetail);
        }

        [HttpPost]
        public JsonResult Update(string name, DateTime startDate, DateTime endDate, string description, int userID, int conventionID, string city)
        {
            var conventionManager = new ConventionManager();
            return Json(conventionManager.Update(name, startDate, endDate, description, userID, conventionID, city));
        }

        public JsonResult Delete(int id)
        {
            var conventionManager = new ConventionManager();
            var status = conventionManager.Delete(id);
            return Json(status);
        }
    }
}