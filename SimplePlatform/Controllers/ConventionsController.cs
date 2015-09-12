using DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

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
            Script = string.Format("conventions.options.isEditDeleteEnable = {0};", IsAdmin.ToString().ToLower());
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
            if (!IsAdmin) { return Json(false); }
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
                Events = GetEventsTarget(modal.ConventionId),
                ProfilePic = modal.FileResource == null ? "" : Url.Content(modal.FileResource.path)
            }).OrderByDescending(modal => modal.StartDate).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
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
            if (!IsAdmin) { return Json(false); }
            var conventionManager = new ConventionManager();
            return Json(conventionManager.Update(name, startDate, endDate, description, userID, conventionID, city));
        }

        public JsonResult Delete(int id)
        {
            if (!IsAdmin) { return Json(false); }
            var conventionManager = new ConventionManager();
            var status = conventionManager.Delete(id);
            return Json(status);
        }
        [HttpPost]
        public JsonResult UploadFile()
        {
            var status = false;
            HttpPostedFileBase myFile = null;
            if (Request.Files.Count > 0) myFile = Request.Files[0];
            if (myFile != null && myFile.ContentLength != 0)
            {
                string pathForSaving = Server.MapPath("~/ImageUploads");
                if (SharedFunction.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        string fileName = DateTime.Now.ToString("MMddyyyyHHmmss") + Path.GetExtension(myFile.FileName);
                        myFile.SaveAs(Path.Combine(pathForSaving, fileName));
                        string path = "~/ImageUploads/" + fileName;
                        if (!IsAdmin) { return Json(false); }
                        var conventionManager = new ConventionManager();
                        status = conventionManager.Add(Request.Form["name"].ToString(), Convert.ToDateTime(Request.Form["startDate"]), Convert.ToDateTime(Request.Form["endDate"]), Request.Form["description"].ToString(), Convert.ToInt32(Request.Form["userId"]), null, path, myFile.FileName);
                    }
                    catch (Exception ex)
                    {
                        return Json(ex.InnerException);
                    }
                }
            }
            return Json(status);
        }
        [HttpPost]
        public JsonResult UpdateFile()
        {
            var status = false;
            HttpPostedFileBase myFile = null;
            if (Request.Files.Count > 0) myFile = Request.Files[0];
            if (myFile != null && myFile.ContentLength != 0)
            {
                string pathForSaving = Server.MapPath("~/ImageUploads");
                if (SharedFunction.CreateFolderIfNeeded(pathForSaving))
                {
                    try
                    {
                        string fileName = DateTime.Now.ToString("MMddyyyyHHmmss") + Path.GetExtension(myFile.FileName);
                        myFile.SaveAs(Path.Combine(pathForSaving, fileName));
                        string path = "~/ImageUploads/" + fileName;
                        var conventionManager = new ConventionManager();
                        status = conventionManager.Update(Request.Form["name"].ToString(), Convert.ToDateTime(Request.Form["startDate"]), Convert.ToDateTime(Request.Form["endDate"]), Request.Form["description"].ToString(), Convert.ToInt32(Request.Form["userId"]), Convert.ToInt32(Request.Form["conventionID"]), null, path, myFile.FileName);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return Json(status);
        }
        public PartialViewResult UploadAttachment(int id)
        {
            var conventionManager = new ConventionManager();
            var conventionDetail = conventionManager.GetConventionDetail(id);
            var conventionAttachments = conventionManager.GetAttachmentListOfConvention(id);
            ViewData["Attachments"] = conventionAttachments;
            return PartialView(conventionDetail);
        }
        [HttpPost]
        public JsonResult UploadAttachment()
        {
            var status = false;
            HttpPostedFileBase myFile = null;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                if (Request.Files.Count > 0) myFile = Request.Files[i];
                if (myFile != null && myFile.ContentLength != 0)
                {
                    string pathForSaving = Server.MapPath("~/AttachmentUploads");
                    if (SharedFunction.CreateFolderIfNeeded(pathForSaving))
                    {
                        try
                        {
                            string fileName = DateTime.Now.ToString("MMddyyyyHHmmss") + Path.GetExtension(myFile.FileName);
                            myFile.SaveAs(Path.Combine(pathForSaving, fileName));
                            string path = "~/AttachmentUploads/" + fileName;
                            if (!IsAdmin) { return Json(false); }
                            var conventionManager = new ConventionManager();
                            status = conventionManager.AddAttachment(Convert.ToInt32(Request.Form["conventionID"].ToString()), path, myFile.FileName);
                        }
                        catch (Exception ex)
                        {
                            return Json(ex.InnerException);
                        }
                    }
                }
            }
            return Json(status);
        }

        public JsonResult DeleteAttachment(int id, int conventionID)
        {
            if (!IsAdmin) { return Json(false); }
            var conventionManager = new ConventionManager();
            var status = conventionManager.DeleteAttachment(id, conventionID);
            return Json(status);
        }
        public ActionResult Detail(int id)
        {
            var convention = new ConventionManager().GetConventionDetail(id);
            //BundleConfig.AddStyle("/Offices", "Detail.css", ControllerName);
            BundleConfig.AddScript("~/Scripts/Conventions", "Detail.js", ControllerName);
            //Script = string.Format("officeDetail.options.officeID = {0};", id);
            return View(convention);
        }
        public JsonResult GetAudiences(int id)
        {
            var audienceManager = new DataModel.ConventionManager();
            var audiences = audienceManager.GetAudiences(id).Select(model => new
            {
                Name = model.Name,
                Contact = model.Contact,
            }).ToList();
            return Json(new { data = audiences });
        }
        public JsonResult GetEvents(int id)
        {
            var audienceManager = new DataModel.ConventionManager();
            var events = audienceManager.GetEvents(id).Select(model => new
            {
                Name = model.Name,
                StartDate = model.StartDate.ToString("MM/dd/yyyy"),
                EndDate = model.EndDate.ToString("MM/dd/yyyy")
            }).ToList();
            return Json(new { data = events });
        }
		public FilePathResult Download(int id)
        {
            var conventionManager = new ConventionManager();
            var convention = conventionManager.GetConventionDetail(id);
            var fileAttachments = convention.ConventionAttachments.Select(model => model.FileResource).ToList();

            var outputDirectory = new DirectoryInfo(string.Format("{0}ExportFiles\\{1}\\{2}", Server.MapPath(@"\"), convention.Name, DateTime.Now.ToString("ddMMyyyyhhmmss")));
            var outputDirectoryPathString = System.IO.Path.Combine(outputDirectory.ToString(), "");
            var isExists = System.IO.Directory.Exists(outputDirectoryPathString);
            if (isExists) System.IO.Directory.Delete(outputDirectoryPathString, true);
            System.IO.Directory.CreateDirectory(outputDirectoryPathString);

            foreach (var fileAttachment in fileAttachments)
            {
                var sourceFilePath = Server.MapPath(fileAttachment.path);
                var destFilePath = System.IO.Path.Combine(outputDirectoryPathString, fileAttachment.name);
                if (!System.IO.Directory.Exists(destFilePath))
                {
                    System.IO.File.Copy(sourceFilePath, destFilePath, true);
                }
            }

            var zipOutputDirectory = new DirectoryInfo(string.Format("{0}ExportFiles\\{1}", Server.MapPath(@"\"), convention.Name));
            var zipOutputDirectoryPathString = System.IO.Path.Combine(zipOutputDirectory.ToString(), (DateTime.Now.ToString("ddMMyyyyhhmmss") + ".zip"));
            ZipFile.CreateFromDirectory(outputDirectoryPathString, zipOutputDirectoryPathString);
            return File(zipOutputDirectoryPathString, "application/zip", DateTime.Now.ToString("ddMMyyyyhhmmss") + ".zip");
        }
    }
}