
#region Using Namespaces
using DataModel.Modal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
#endregion

namespace DataAccess
{
    public class EventManager : DBManager
    {
        public bool Add(string name, DateTime startDate, DateTime endDate, string description, int officeID, int conventionID, string city)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_AddEvent]"))
                {
                    database.AddInParameter(command, "@name", DbType.String, name);
                    database.AddInParameter(command, "@startDate", DbType.DateTime, startDate);
                    database.AddInParameter(command, "@endDate", DbType.DateTime, endDate);
                    database.AddInParameter(command, "@description", DbType.String, description);
                    database.AddInParameter(command, "@officeID", DbType.Int32, officeID);
                    database.AddInParameter(command, "@conventionID", DbType.Int32, conventionID);
                    database.AddInParameter(command, "@city", DbType.String, city);
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

        public bool Update(string name, DateTime startDate, DateTime endDate, string description, int officeID, int eventID, int conventionID, string city)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_UpdateEvent]"))
                {
                    database.AddInParameter(command, "@eventID", DbType.Int32, eventID);
                    database.AddInParameter(command, "@name", DbType.String, name);
                    database.AddInParameter(command, "@startDate", DbType.DateTime, startDate);
                    database.AddInParameter(command, "@endDate", DbType.DateTime, endDate);
                    database.AddInParameter(command, "@description", DbType.String, description);
                    database.AddInParameter(command, "@officeID", DbType.Int32, officeID);
                    database.AddInParameter(command, "@conventionID", DbType.Int32, conventionID);
                    database.AddInParameter(command, "@city", DbType.String, city);
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

        public bool Delete(int eventID)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_DeleteEvent]"))
                {
                    database.AddInParameter(command, "@eventID", DbType.Int32, eventID);
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

        public List<Event> GetEvents(List<int> officeIDs, DateTime startDate, DateTime endDate)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetEventByOfficeIDs]"))
                {
                    database.AddInParameter(command, "@officeIDs", DbType.String, String.Join("|", officeIDs.ToArray()));
                    database.AddInParameter(command, "@StartDate", DbType.DateTime, startDate);
                    database.AddInParameter(command, "@EndDate", DbType.DateTime, endDate);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var events = (from dataRow in dataTable.AsEnumerable()
                              select new DataModel.Modal.Event
                              {
                                  EventId = dataRow.Field<int>("EventId"),
                                  Name = dataRow.Field<string>("Name"),
                                  Description = dataRow.Field<string>("Description"),
                                  StartDate = dataRow.Field<DateTime>("StartDate"),
                                  EndDate = dataRow.Field<DateTime>("EndDate"),
                                  City = dataRow.Field<string>("City"),
                                  TotalAttended = dataRow.Field<int>("TotalAttended"),
                                  convention = new Convention { ConventionId = dataRow.Field<int>("ConventionId"), Name = dataRow.Field<string>("ConventionName") },
                                  Office = new Office
                                  {
                                      OfficeId = dataRow.Field<Int32>("OfficeId"),
                                      Name = dataRow.Field<String>("OfficeName"),
                                      FileResource = dataRow.Field<int?>("FileResourceID").GetValueOrDefault(0) == 0 ? null : new FileResource
                                      {
                                          Id = dataRow.Field<Int32>("FileResourceID"),
                                          path = dataRow.Field<String>("FileResourcePath"),
                                      }
                                  }
                              }).ToList();
                return events;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Event> GetActiveEvents(List<int> officeIDs)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetActiveEventByOfficeIDs]"))
                {
                    database.AddInParameter(command, "@officeIDs", DbType.String, String.Join("|", officeIDs.ToArray()));
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var events = (from dataRow in dataTable.AsEnumerable()
                              select new DataModel.Modal.Event
                              {
                                  EventId = dataRow.Field<int>("EventId"),
                                  Name = dataRow.Field<string>("Name"),
                                  Description = dataRow.Field<string>("Description"),
                                  StartDate = dataRow.Field<DateTime>("StartDate"),
                                  EndDate = dataRow.Field<DateTime>("EndDate"),
                                  City = dataRow.Field<string>("City"),
                                  TotalAttended = dataRow.Field<int>("TotalAttended"),
                                  convention = new Convention { ConventionId = dataRow.Field<int>("ConventionId"), Name = dataRow.Field<string>("ConventionName") },
                                  Office = new Office
                                  {
                                      OfficeId = dataRow.Field<Int32>("OfficeId"),
                                      Name = dataRow.Field<String>("OfficeName"),
                                      FileResource = dataRow.Field<int?>("FileResourceID").GetValueOrDefault(0) == 0 ? null : new FileResource
                                      {
                                          Id = dataRow.Field<Int32>("FileResourceID"),
                                          path = dataRow.Field<String>("FileResourcePath"),
                                      }
                                  }
                              }).ToList();
                return events;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Event GetEventDetail(int id)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetEventByID]"))
                {
                    database.AddInParameter(command, "@ID", DbType.String, id);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var eventDetail = (from dataRow in dataTable.AsEnumerable()
                                   select new DataModel.Modal.Event
                                   {
                                       EventId = dataRow.Field<int>("EventId"),
                                       Name = dataRow.Field<string>("Name"),
                                       Description = dataRow.Field<string>("Description"),
                                       StartDate = dataRow.Field<DateTime>("StartDate"),
                                       EndDate = dataRow.Field<DateTime>("EndDate"),
                                       City = dataRow.Field<string>("City"),
                                       TotalAttended = dataRow.Field<int>("TotalAttended"),
                                       convention = new Convention { ConventionId = dataRow.Field<int>("ConventionId"), Name = dataRow.Field<string>("ConventionName") },
                                       Office = new Office
                                       {
                                           OfficeId = dataRow.Field<Int32>("OfficeId"),
                                           Name = dataRow.Field<String>("OfficeName"),
                                           FileResource = dataRow.Field<int?>("FileResourceID").GetValueOrDefault(0) == 0 ? null : new FileResource
                                           {
                                               Id = dataRow.Field<Int32>("FileResourceID"),
                                               path = dataRow.Field<String>("FileResourcePath"),
                                           }
                                       },
                                   }).FirstOrDefault();

                var audienceManager = new AudienceManager();
                eventDetail.Audiences = audienceManager.GetAudiencesByEventID(eventDetail.EventId);
                return eventDetail;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Event> GetEvents(int conventionID)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetEventByConventionID]"))
                {
                    database.AddInParameter(command, "@conventionID", DbType.Int32, conventionID);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var events = (from dataRow in dataTable.AsEnumerable()
                              select new DataModel.Modal.Event
                              {
                                  EventId = dataRow.Field<int>("EventId"),
                                  Name = dataRow.Field<string>("Name"),
                                  Description = dataRow.Field<string>("Description"),
                                  StartDate = dataRow.Field<DateTime>("StartDate"),
                                  EndDate = dataRow.Field<DateTime>("EndDate"),
                                  City = dataRow.Field<string>("City")
                              }).ToList();
                return events;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
