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
    public class TasksController : BaseController
    {
        public ActionResult Index()
        {
            BundleConfig.AddScript("~/Scripts/Tasks", "tasks.js", ControllerName);
            BundleConfig.AddStyle("/Tasks", "tasks.css", ControllerName);

            return View();
        }

        public JsonResult GetTasks()
        {
            var taskManager = new DataModel.TaskManager();
            var isOfficeAdmin = UserDetail.User.Roles.Any(role => new List<int> { 1, 2 }.Contains(role.RoleId));
            List<DataModel.Modal.Task> taskList;
            if (IsAdmin)
            {
                taskList = taskManager.GetTasks().Where(model => model.UsersDetail == null).ToList();
            }
            else if (isOfficeAdmin)
            {
                var officeIDs = UserDetail.Offices.Where(model => model.IsDeleted == false).Select(model => model.OfficeId).ToList();
                taskList = taskManager.GetTasks().Where(model => officeIDs.Contains(model.Office.OfficeId)).ToList();
            }
            else
            {
                taskList = UserDetail.Tasks.ToList();
            }
            var tasks = taskList.Select(model => new
            {
                ID = model.TaskId,
                Title = model.Name,
                DueDate = model.EndDate.ToString("dd-MM-yyyy"),
                AssignTo = (model.UsersDetail == null ? model.Office.Name : (model.UsersDetail.UserId == UserDetail.UserId) ? "Me" : (model.UsersDetail.User.FirstName + " " + model.UsersDetail.User.LastName)),
                Status = model.IsCompleted,
                OfficeID = model.Office.OfficeId,
                UserID = (model.UsersDetail == null ? 0 : model.UsersDetail.UserId),
            }).ToList();
            return Json(new
            {
                data = tasks
            });
        }

        public ActionResult Add()
        {
            if (!IsAdmin) { return RedirectToAction("AddByOffice"); }
            var officeMananer = new DataModel.OfficeMananer();
            var offices = IsAdmin ? officeMananer.GetOffices() : UserDetail.Offices.ToList();
            ViewData["Offices"] = offices.Where(model => model.IsDeleted == false).ToList();
            return PartialView();
        }

        public ActionResult AddByOffice()
        {
            var offices = UserDetail.Offices.ToList();
            ViewData["Offices"] = offices.Where(model => model.IsDeleted == false).ToList();
            ViewData["UserID"] = UserDetail.UserId;
            return PartialView();
        }

        [HttpPost]
        public JsonResult Add(string name, string startDate, string endDate, string description, int officeID, int userID)
        {
            var taskManager = new DataModel.TaskManager();
            var status = taskManager.Add(name, startDate, endDate, description, officeID, userID);
            return Json(status);
        }

        public ActionResult Edit(int id)
        {
            var officeMananer = new DataModel.OfficeMananer();
            var taskManager = new DataModel.TaskManager();
            var task = taskManager.GetTask(id);
            var offices = IsAdmin ? officeMananer.GetOffices() : UserDetail.Offices.ToList();
            ViewData["Offices"] = offices.Where(model => model.IsDeleted == false).ToList();
            return PartialView(task);
        }

        public JsonResult Update(int taskID, string name, string startDate, string endDate, string description, int officeID, int userID)
        {
            var taskManager = new DataModel.TaskManager();
            var status = taskManager.Update(taskID, name, startDate, endDate, description, officeID, userID);
            return Json(status);
        }

        public JsonResult Delete(int id)
        {
            var taskManager = new DataModel.TaskManager();
            var status = taskManager.Delete(id);
            return Json(status);
        }

        public PartialViewResult GetDetail(int id)
        {
            var taskManager = new DataModel.TaskManager();
            var task = taskManager.GetTask(id);
            return PartialView(task);
        }

        public JsonResult Status(int id)
        {
            var taskManager = new DataModel.TaskManager();
            var status = taskManager.Status(id);
            return Json(status);
        }

        public PartialViewResult UploadAttachment(int id)
        {
            var taskManager = new DataModel.TaskManager();
            var taskDetail = taskManager.GetTask(id);
            return PartialView(taskDetail);
        }

        [HttpPost]
        public JsonResult UploadAttachment()
        {
            var status = false;
            HttpPostedFileBase myFile = null;
            var fileResources = new List<DataModel.Modal.CommentAttachment>();
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
                            fileResources.Add(new DataModel.Modal.CommentAttachment { FileResource = new DataModel.Modal.FileResource { name = myFile.FileName, path = path } });
                        }
                        catch (Exception ex)
                        {
                            return Json(ex.InnerException);
                        }
                    }
                }
            }

            if (!IsAdmin) { return Json(false); }
            var commentManager = new DataModel.CommentManager();
            status = commentManager.Add(Convert.ToInt32(Request.Form["taskID"].ToString()), UserDetail.UserId, Request.Form["comment"].ToString(), fileResources);
            return Json(status);
        }

        public FilePathResult Download(int id)
        {
            var commentManager = new DataModel.CommentManager();
            var comment = commentManager.GetComment(id);
            var fileAttachments = comment.CommentAttachments.Select(model => model.FileResource).ToList();

            var outputDirectory = new DirectoryInfo(string.Format("{0}ExportFiles\\{1}\\{2}", Server.MapPath(@"\"), comment.CommentId, DateTime.Now.ToString("ddMMyyyyhhmmss")));
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

            var zipOutputDirectory = new DirectoryInfo(string.Format("{0}ExportFiles\\{1}", Server.MapPath(@"\"), comment.CommentId));
            var zipOutputDirectoryPathString = System.IO.Path.Combine(zipOutputDirectory.ToString(), (DateTime.Now.ToString("ddMMyyyyhhmmss") + ".zip"));
            ZipFile.CreateFromDirectory(outputDirectoryPathString, zipOutputDirectoryPathString);
            return File(zipOutputDirectoryPathString, "application/zip", DateTime.Now.ToString("ddMMyyyyhhmmss") + ".zip");
        }
    }
}