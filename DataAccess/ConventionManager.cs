using DataModel.Modal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ConventionManager : DBManager
    {
        public bool Add(string name, DateTime startDate, DateTime endDate, string description, int userID, string city, string path, string fileName)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_AddConvention]"))
                {
                    database.AddInParameter(command, "@name", DbType.String, name);
                    database.AddInParameter(command, "@startDate", DbType.DateTime, startDate);
                    database.AddInParameter(command, "@endDate", DbType.DateTime, endDate);
                    database.AddInParameter(command, "@description", DbType.String, description);
                    database.AddInParameter(command, "@city", DbType.String, city);
                    if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(fileName))
                    {
                        database.AddInParameter(command, "@path", DbType.String, path);
                        database.AddInParameter(command, "@fileName", DbType.String, fileName);
                    }
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

        public bool Add(string name, DateTime startDate, DateTime endDate, string description, int userID, string city)
        {
            return Add(name, startDate, endDate, description, userID, city, string.Empty, string.Empty);
        }

        public bool Update(string name, DateTime startDate, DateTime endDate, string description, int userID, int conventionID, string city, string path, string fileName)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_UpdateConvention]"))
                {
                    database.AddInParameter(command, "@conventionID", DbType.Int32, conventionID);
                    database.AddInParameter(command, "@name", DbType.String, name);
                    database.AddInParameter(command, "@startDate", DbType.DateTime, startDate);
                    database.AddInParameter(command, "@endDate", DbType.DateTime, endDate);
                    database.AddInParameter(command, "@description", DbType.String, description);
                    database.AddInParameter(command, "@city", DbType.String, city);
                    if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(fileName))
                    {
                        database.AddInParameter(command, "@path", DbType.String, path);
                        database.AddInParameter(command, "@fileName", DbType.String, fileName);
                    }
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

        public bool Update(string name, DateTime startDate, DateTime endDate, string description, int userID, int conventionID, string city)
        {
            return Update(name, startDate, endDate, description, userID, conventionID, city, string.Empty, string.Empty);
        }

        public bool Delete(int id)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_DeleteConvention]"))
                {
                    database.AddInParameter(command, "@conventionID", DbType.Int32, id);
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

        public bool AddAttachment(int conventionID, string path, string fileName)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_AddAttachmentConvention]"))
                {
                    database.AddInParameter(command, "@conventionID", DbType.Int32, conventionID);
                    database.AddInParameter(command, "@path", DbType.String, path);
                    database.AddInParameter(command, "@fileName", DbType.String, fileName);
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

        public bool DeleteAttachment(int id, int conventionID)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_DeleteAttachmentConvention]"))
                {
                    database.AddInParameter(command, "@conventionID", DbType.Int32, conventionID);
                    database.AddInParameter(command, "@conventionAttachmentID", DbType.Int32, id);
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

        public List<Convention> GetConventions()
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetConventions]"))
                {
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var comments = (from dataRow in dataTable.AsEnumerable()
                                select new DataModel.Modal.Convention
                                {
                                    ConventionId = dataRow.Field<int>("ConventionId"),
                                    Name = dataRow.Field<string>("Name"),
                                    StartDate = dataRow.Field<DateTime>("StartDate"),
                                    EndDate = dataRow.Field<DateTime>("EndDate"),
                                    Description = dataRow.Field<string>("Description"),
                                    City = dataRow.Field<string>("City"),
                                    IsFileAttached = dataRow.Field<int>("IsAttachment") != 0,
                                    FileResource = dataRow.Field<int?>("FileResourcesID").GetValueOrDefault(0) == 0 ? null : new FileResource { Id = dataRow.Field<int>("FileResourcesID"), name = dataRow.Field<string>("FileResourcesName"), path = dataRow.Field<string>("FileResourcesPath") }
                                }).ToList();
                return comments;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ConventionAttachment> GetAttachmentListOfConvention(int id)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetConventionAttachmentsByID]"))
                {
                    database.AddInParameter(command, "@ConventionID", DbType.Int32, id);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var conventionAttachments = (from dataRow in dataTable.AsEnumerable()
                                select new DataModel.Modal.ConventionAttachment
                                {
                                    ConventionAttachmentId = dataRow.Field<int>("ConventionAttachmentId"),
                                    Convention =  new Convention { ConventionId = dataRow.Field<int>("Convention_ConventionId") },
                                    FileResource = new FileResource { Id = dataRow.Field<int>("FileResourceID"), name = dataRow.Field<string>("FileResourceName"), path = dataRow.Field<string>("FileResourcePath") }
                                }).ToList();
                return conventionAttachments;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Convention> GetActiveConventions()
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetActiveConventions]"))
                {
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var comments = (from dataRow in dataTable.AsEnumerable()
                                select new DataModel.Modal.Convention
                                {
                                    ConventionId = dataRow.Field<int>("ConventionId"),
                                    Name = dataRow.Field<string>("Name"),
                                    StartDate = dataRow.Field<DateTime>("StartDate"),
                                    EndDate = dataRow.Field<DateTime>("EndDate"),
                                    Description = dataRow.Field<string>("Description"),
                                    City = dataRow.Field<string>("City"),
                                    IsFileAttached = dataRow.Field<int>("IsAttachment") != 0,
                                    FileResource = dataRow.Field<int?>("FileResourcesID").GetValueOrDefault(0) == 0 ? null : new FileResource { Id = dataRow.Field<int>("FileResourcesID"), name = dataRow.Field<string>("FileResourcesName"), path = dataRow.Field<string>("FileResourcesPath") }
                                }).ToList();
                return comments;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Convention GetConventionDetail(int id)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetConventionDetailByID]"))
                {
                    database.AddInParameter(command, "@ConventionID", DbType.Int32, id);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var convention = (from dataRow in dataTable.AsEnumerable()
                                select new DataModel.Modal.Convention
                                {
                                    ConventionId = dataRow.Field<int>("ConventionId"),
                                    Name = dataRow.Field<string>("Name"),
                                    StartDate = dataRow.Field<DateTime>("StartDate"),
                                    EndDate = dataRow.Field<DateTime>("EndDate"),
                                    Description = dataRow.Field<string>("Description"),
                                    City = dataRow.Field<string>("City"),
                                    IsFileAttached = dataRow.Field<int>("IsAttachment") != 0,
                                    FileResource = dataRow.Field<int?>("FileResourcesID").GetValueOrDefault(0) == 0 ? null : new FileResource { Id = dataRow.Field<int>("FileResourcesID"), name = dataRow.Field<string>("FileResourcesName"), path = dataRow.Field<string>("FileResourcesPath") }
                                }).FirstOrDefault();
                return convention;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

