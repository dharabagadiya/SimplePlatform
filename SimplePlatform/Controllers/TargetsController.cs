﻿
#region Using Regions
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
#endregion

namespace SimplePlatform.Controllers
{
    public class TargetsController : BaseController
    {
        // GET: Targets
        public ActionResult Index()
        {
            var officeManager = new DataAccess.OfficeMananer();
            ViewData["Offices"] = officeManager.GetOffices(IsAdmin ? 0 : UserDetail.UserId);
            BundleConfig.AddScript("~/Scripts/Offices", "targets.js", ControllerName);
            return View();
        }

        public JsonResult GetTasks()
        {
            var targetManager = new DataAccess.TargetManager();
            var targetList = targetManager.GetTargets();
            var targets = targetList.Select(model => new
            {
                ID = model.TargetId,
                OfficeName = model.Office.Name,
                DueDate = model.DueDate.ToString("MMM dd,yyyy"),
                Booking = model.Booking,
                FundRaising = model.FundRaising,
                GSB = model.GSB,
                Arrivals = model.Arrivals
            }).ToList();
            return Json(new { data = targets });
        }

        public JsonResult Add(int officeID, string dueDate, int bookingTargets, float fundRaisingAmount, float gsbAmount, int arrivalTargets)
        {
            var targetManager = new DataAccess.TargetManager();
            var dueDateDateTime = Convert.ToDateTime(dueDate);
            var status = targetManager.Add(officeID, dueDateDateTime, bookingTargets, fundRaisingAmount, gsbAmount, arrivalTargets);
            return Json(status);
        }

        public PartialViewResult Edit(int id)
        {
            var officeManager = new DataAccess.OfficeMananer();
            ViewData["Offices"] = officeManager.GetOffices(IsAdmin ? 0 : UserDetail.UserId);
            var targetManager = new DataAccess.TargetManager();
            var target = targetManager.GetTarget(id);
            return PartialView(target);
        }

        public JsonResult Update(int targetID, int officeID, string dueDate, int bookingTargets, float fundRaisingAmount, float gsbAmount, int arrivalTargets)
        {
            var targetManager = new DataAccess.TargetManager();
            var dueDateDateTime = Convert.ToDateTime(dueDate);
            var status = targetManager.Update(targetID, officeID, dueDateDateTime, bookingTargets, fundRaisingAmount, gsbAmount, arrivalTargets);
            return Json(status);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var targetManager = new DataAccess.TargetManager();
            var status = targetManager.Delete(id);
            return Json(status);
        }
    }
}