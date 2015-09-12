﻿using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePlatform.Controllers
{
    public class EventsController : BaseController
    {
        public ActionResult Index()
        {
            BundleConfig.AddScript("~/Scripts/Events", "Events.js", ControllerName);
            return View();
        }
        public ActionResult Add()
        {
            var userDetailManager = new UserManager();
            var officeMananer = new OfficeMananer();
            var user = userDetailManager.GetUserDetail(UserDetail.UserId);
            var offices = IsAdmin ? officeMananer.GetOffices() : user.Offices.ToList();
            ViewData["Offices"] = offices;
            var conventionManager = new ConventionManager();
            var conventions = conventionManager.GetConventions();
            ViewData["Conventions"] = conventions;
            if (IsAdmin)
            {
                var customRoleProvider = new CustomAuthentication.CustomRoleProvider();
                var customMembershipProvider = new CustomAuthentication.CustomMembershipProvider();
                ViewData["Employee"] = customMembershipProvider.GetUsers(4);
            }
            return PartialView();
        }
        [HttpPost]
        public JsonResult Add(string name, DateTime startDate, DateTime endDate, string description, int officeID, int conventionID, string city)
        {
            var eventManager = new EventManager();
            return Json(eventManager.Add(name, startDate, endDate, description, officeID, conventionID, city));
        }
        public JsonResult GetEvents()
        {
            var isUpdateEnable = UserDetail.User.Roles.Any(role => new List<int> { 1, 2 }.Contains(role.RoleId));
            var officesManager = new OfficeMananer();
            var offices = IsAdmin ? officesManager.GetOffices() : UserDetail.Offices;
            var eventList = offices.Where(model => model.IsDeleted == false).SelectMany(model => model.Events).ToList();
            var events = eventList.Where(model => model.IsDeleted == false).Select(modal => new
            {
                id = modal.EventId,
                name = modal.Name,
                startDate = modal.StartDate.ToString("dd-MM-yyyy HH:mm"),
                endDate = modal.EndDate.ToString("dd-MM-yyyy HH:mm"),
                description = modal.Description,
                city = modal.City,
                IsUpdateEnable = isUpdateEnable
            }).ToList();
            return Json(new { data = events });
        }
        public PartialViewResult Edit(int id)
        {
            var userDetailManager = new UserManager();
            var officeMananer = new OfficeMananer();
            var user = userDetailManager.GetUserDetail(UserDetail.UserId);
            var offices = IsAdmin ? officeMananer.GetOffices() : user.Offices.ToList();
            ViewData["Offices"] = offices;
            var conventionManager = new ConventionManager();
            var conventions = conventionManager.GetConventions();
            ViewData["Conventions"] = conventions;
            var eventManager = new EventManager();
            var eventDetail = eventManager.GetEventDetail(id);
            return PartialView(eventDetail);
        }
        [HttpPost]
        public JsonResult Update(string name, DateTime startDate, DateTime endDate, string description, int officeID, int eventID, int conventionID, string city)
        {
            var eventManager = new EventManager();
            return Json(eventManager.Update(name, startDate, endDate, description, officeID, eventID, conventionID, city));
        }
        public JsonResult Delete(int id)
        {
            var eventManager = new EventManager();
            var status = eventManager.Delete(id);
            return Json(status);
        }
    }
}