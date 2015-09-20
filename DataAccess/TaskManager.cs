using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TaskManager : DBManager
    {
        private DataModel.Modal.Task GetTaskDetail(int id)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetTaskByID]"))
                {
                    database.AddInParameter(command, "@ID", DbType.Int32, id);
                    dataSet = database.ExecuteDataSet(command);
                }
                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var task = (from dataRow in dataTable.AsEnumerable()
                             select new DataModel.Modal.Task
                             {
                                 TaskId = dataRow.Field<int>("TaskId"),
                                 Name = dataRow.Field<string>("Name"),
                                 StartDate = dataRow.Field<DateTime>("StartDate"),
                                 EndDate = dataRow.Field<DateTime>("EndDate"),
                                 Description = dataRow.Field<string>("Description"),
                                 IsCompleted = dataRow.Field<bool>("IsCompleted"),
                                 CreateDate = dataRow.Field<DateTime>("CreateDate"),
                                 UpdateDate = dataRow.Field<DateTime>("UpdateDate"),
                                 Office = new DataModel.Modal.Office { OfficeId = dataRow.Field<int>("OfficeId"), Name = dataRow.Field<string>("OfficeName") },
                                 UsersDetail = dataRow.Field<int?>("UserId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.UserDetail
                                 {
                                     UserId = dataRow.Field<int>("UserId"),
                                     User = new CustomAuthentication.User
                                     {
                                         UserId = dataRow.Field<int>("UserId"),
                                         FirstName = dataRow.Field<string>("FirstName"),
                                         LastName = dataRow.Field<string>("LastName")
                                     }
                                 }
                             }).FirstOrDefault();
                return task;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Add(string name, string startDates, string endDates, string description, int officeID, int userID)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_AddTask]"))
                {
                    database.AddInParameter(command, "@Name", DbType.String, name);
                    database.AddInParameter(command, "@StartDate", DbType.DateTime, startDates);
                    database.AddInParameter(command, "@EndDate", DbType.DateTime, endDates);
                    database.AddInParameter(command, "@Description", DbType.String, description);
                    database.AddInParameter(command, "@UserId", DbType.Int32, userID);
                    database.AddInParameter(command, "@OfficeId", DbType.Int32, officeID);
                    database.AddOutParameter(command, "@Status", DbType.Int32, returnVale);
                    database.ExecuteNonQuery(command);
                    returnVale = (int)database.GetParameterValue(command, "@Status");
                }
                return returnVale == 1;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(int taskID, string name, string startDates, string endDates, string description, int officeID, int userID)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_UpdateTask]"))
                {
                    database.AddInParameter(command, "@ID", DbType.Int32, taskID);
                    database.AddInParameter(command, "@Name", DbType.String, name);
                    database.AddInParameter(command, "@StartDate", DbType.DateTime, startDates);
                    database.AddInParameter(command, "@EndDate", DbType.DateTime, endDates);
                    database.AddInParameter(command, "@Description", DbType.String, description);
                    database.AddInParameter(command, "@UserId", DbType.Int32, userID);
                    database.AddInParameter(command, "@OfficeId", DbType.Int32, officeID);
                    database.AddOutParameter(command, "@Status", DbType.Int32, returnVale);
                    database.ExecuteNonQuery(command);
                    returnVale = (int)database.GetParameterValue(command, "@Status");
                }
                return returnVale == 1;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int taskID)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_DeleteTask]"))
                {
                    database.AddInParameter(command, "@ID", DbType.Int32, taskID);
                    database.AddOutParameter(command, "@Status", DbType.Int32, returnVale);
                    database.ExecuteNonQuery(command);
                    returnVale = (int)database.GetParameterValue(command, "@Status");
                }
                return returnVale == 1;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Status(int taskID)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_UpdateTaskStatus]"))
                {
                    database.AddInParameter(command, "@ID", DbType.Int32, taskID);
                    database.AddOutParameter(command, "@Status", DbType.Int32, returnVale);
                    database.ExecuteNonQuery(command);
                    returnVale = (int)database.GetParameterValue(command, "@Status");
                }
                return returnVale == 1;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<DataModel.Modal.Task> GetTasks(DateTime startDate, DateTime endDate, int userID)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetTasksByUserID]"))
                {
                    database.AddInParameter(command, "@StartDate", DbType.DateTime, startDate);
                    database.AddInParameter(command, "@EndDate", DbType.DateTime, endDate);
                    database.AddInParameter(command, "@UserID", DbType.Int32, userID);
                    dataSet = database.ExecuteDataSet(command);
                }
                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var tasks = (from dataRow in dataTable.AsEnumerable()
                             select new DataModel.Modal.Task
                             {
                                 TaskId = dataRow.Field<int>("TaskId"),
                                 Name = dataRow.Field<string>("Name"),
                                 StartDate = dataRow.Field<DateTime>("StartDate"),
                                 EndDate = dataRow.Field<DateTime>("EndDate"),
                                 Description = dataRow.Field<string>("Description"),
                                 IsCompleted = dataRow.Field<bool>("IsCompleted"),
                                 CreateDate = dataRow.Field<DateTime>("CreateDate"),
                                 UpdateDate = dataRow.Field<DateTime>("UpdateDate"),
                                 Office = new DataModel.Modal.Office { OfficeId = dataRow.Field<int>("OfficeId"), Name = dataRow.Field<string>("OfficeName") },
                                 UsersDetail = dataRow.Field<int?>("UserId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.UserDetail
                                 {
                                     UserId = dataRow.Field<int>("UserId"),
                                     User = new CustomAuthentication.User
                                     {
                                         UserId = dataRow.Field<int>("UserId"),
                                         FirstName = dataRow.Field<string>("FirstName"),
                                         LastName = dataRow.Field<string>("LastName")
                                     }
                                 }
                             }).ToList();
                return tasks;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DataModel.Modal.Task> GetTasks(DateTime startDate, DateTime endDate, int officeID, int userID)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetTasksByOfficeIDUserID]"))
                {
                    database.AddInParameter(command, "@StartDate", DbType.DateTime, startDate);
                    database.AddInParameter(command, "@EndDate", DbType.DateTime, endDate);
                    database.AddInParameter(command, "@OfficeID", DbType.Int32, officeID);
                    database.AddInParameter(command, "@UserID", DbType.Int32, userID);
                    dataSet = database.ExecuteDataSet(command);
                }
                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var tasks = (from dataRow in dataTable.AsEnumerable()
                             select new DataModel.Modal.Task
                             {
                                 TaskId = dataRow.Field<int>("TaskId"),
                                 Name = dataRow.Field<string>("Name"),
                                 StartDate = dataRow.Field<DateTime>("StartDate"),
                                 EndDate = dataRow.Field<DateTime>("EndDate"),
                                 Description = dataRow.Field<string>("Description"),
                                 IsCompleted = dataRow.Field<bool>("IsCompleted"),
                                 CreateDate = dataRow.Field<DateTime>("CreateDate"),
                                 UpdateDate = dataRow.Field<DateTime>("UpdateDate"),
                                 Office = new DataModel.Modal.Office { OfficeId = dataRow.Field<int>("OfficeId"), Name = dataRow.Field<string>("OfficeName") },
                                 UsersDetail = dataRow.Field<int?>("UserId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.UserDetail
                                 {
                                     UserId = dataRow.Field<int>("UserId"),
                                     User = new CustomAuthentication.User
                                     {
                                         UserId = dataRow.Field<int>("UserId"),
                                         FirstName = dataRow.Field<string>("FirstName"),
                                         LastName = dataRow.Field<string>("LastName")
                                     }
                                 }
                             }).ToList();
                return tasks;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataModel.Modal.Task GetTask(int id)
        {
            var commentManager = new CommentManager();
            var taskDetail = GetTaskDetail(id);
            taskDetail.Comments = commentManager.GetComments(id);
            return taskDetail;
        }
    }
}