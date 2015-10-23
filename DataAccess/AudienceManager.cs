
#region Using Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace DataAccess
{
    public class AudienceManager : DBManager
    {
        public bool Add(string name, string contact, DateTime visitDate, int visitTypeID, int officeID, int eventID, int fsmID, int conventionID, int serviceID, int bookingStatus, float GSBAmount, float amount)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_AddAudience]"))
                {
                    database.AddInParameter(command, "@Name", DbType.String, name);
                    database.AddInParameter(command, "@Contact", DbType.String, contact);
                    database.AddInParameter(command, "@VisitDate", DbType.DateTime, visitDate);
                    database.AddInParameter(command, "@VisitTypeID", DbType.Int32, visitTypeID);
                    database.AddInParameter(command, "@OfficeID", DbType.Int32, officeID);
                    database.AddInParameter(command, "@EventID", DbType.Int32, eventID);
                    database.AddInParameter(command, "@FSMID", DbType.Int32, fsmID);
                    database.AddInParameter(command, "@ConventionID", DbType.Int32, conventionID);
                    database.AddInParameter(command, "@ServiceID", DbType.Int32, serviceID);
                    database.AddInParameter(command, "@BookingStatus", DbType.Int32, bookingStatus);
                    database.AddInParameter(command, "@GSBAmount", DbType.Single, GSBAmount);
                    database.AddInParameter(command, "@Amount", DbType.Single, amount);
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

        public bool Update(int audienceID, string name, string contact, DateTime visitDate, int visitTypeID, int officeID, int eventID, int fsmID, int conventionID, int serviceID, int bookingStatus, float GSBAmount, float amount)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_UpdateAudience]"))
                {
                    database.AddInParameter(command, "@AudienceID", DbType.Int32, audienceID);
                    database.AddInParameter(command, "@Name", DbType.String, name);
                    database.AddInParameter(command, "@Contact", DbType.String, contact);
                    database.AddInParameter(command, "@VisitDate", DbType.DateTime, visitDate);
                    database.AddInParameter(command, "@VisitTypeID", DbType.Int32, visitTypeID);
                    database.AddInParameter(command, "@OfficeID", DbType.Int32, officeID);
                    database.AddInParameter(command, "@EventID", DbType.Int32, eventID);
                    database.AddInParameter(command, "@FSMID", DbType.Int32, fsmID);
                    database.AddInParameter(command, "@ConventionID", DbType.Int32, conventionID);
                    database.AddInParameter(command, "@ServiceID", DbType.Int32, serviceID);
                    database.AddInParameter(command, "@BookingStatus", DbType.Int32, bookingStatus);
                    database.AddInParameter(command, "@GSBAmount", DbType.Single, GSBAmount);
                    database.AddInParameter(command, "@Amount", DbType.Single, amount);
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

        public bool Delete(int id)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_DeleteAudience]"))
                {
                    database.AddInParameter(command, "@AudienceID", DbType.Int32, id);
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

        public bool AttendStatus(int id)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_UpdateAudienceStatus]"))
                {
                    database.AddInParameter(command, "@AudienceID", DbType.Int32, id);
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

        public DataModel.Modal.Audience GetAudience(int id)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetAudienceByID]"))
                {
                    database.AddInParameter(command, "@ID", DbType.Int32, id);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var audience = (from dataRow in dataTable.AsEnumerable()
                                select new DataModel.Modal.Audience
                                {
                                    AudienceID = dataRow.Field<int>("AudienceID"),
                                    Name = dataRow.Field<String>("Name"),
                                    Contact = dataRow.Field<String>("Contact"),
                                    VisitDate = dataRow.Field<DateTime>("VisitDate"),
                                    GSBAmount = dataRow.Field<float>("GSBAmount"),
                                    IsAttended = dataRow.Field<bool>("IsAttended"),
                                    Amount = dataRow.Field<float>("Amount"),
                                    BookingStatus = dataRow.Field<int>("BookingStatus"),
                                    FSMDetail = dataRow.Field<int?>("FSMDetailID").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.FSMDetail { ID = dataRow.Field<int>("FSMDetailID"), Name = dataRow.Field<String>("FSMDetailName"), PhoneNumber = dataRow.Field<String>("FSMDetailPhoneNumber") },
                                    Office = new DataModel.Modal.Office { OfficeId = dataRow.Field<int>("OfficeId"), Name = dataRow.Field<String>("OfficeName") },
                                    Event = dataRow.Field<int?>("EventId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.Event { EventId = dataRow.Field<int>("EventId"), Name = dataRow.Field<String>("EventName") },
                                    Convention = dataRow.Field<int?>("ConventionId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.Convention { ConventionId = dataRow.Field<int>("ConventionId"), Name = dataRow.Field<String>("ConventionName") },
                                    VisitType = dataRow.Field<int?>("VisitTypeId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.VisitType { VisitTypeId = dataRow.Field<int>("VisitTypeId"), VisitTypeName = dataRow.Field<String>("VisitTypeName") },
                                    Service = dataRow.Field<int?>("ServiceId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.Service { ServiceId = dataRow.Field<int>("ServiceId"), ServiceName = dataRow.Field<String>("ServiceName") }
                                }).FirstOrDefault();
                return audience;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DataModel.Modal.Audience> GetAudiences(List<int> officeIDs, DateTime startDate, DateTime endDate)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetAudienceByOfficeIDs]"))
                {
                    database.AddInParameter(command, "@IDs", DbType.String, String.Join("|", officeIDs.ToArray()));
                    database.AddInParameter(command, "@StartDate", DbType.DateTime, startDate);
                    database.AddInParameter(command, "@EndDate", DbType.DateTime, endDate);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var audiences = (from dataRow in dataTable.AsEnumerable()
                                 select new DataModel.Modal.Audience
                                 {
                                     AudienceID = dataRow.Field<int>("AudienceID"),
                                     Name = dataRow.Field<String>("Name"),
                                     Contact = dataRow.Field<String>("Contact"),
                                     VisitDate = dataRow.Field<DateTime>("VisitDate"),
                                     GSBAmount = dataRow.Field<float>("GSBAmount"),
                                     IsAttended = dataRow.Field<bool>("IsAttended"),
                                     FSMName = dataRow.Field<String>("FSMName"),
                                     Amount = dataRow.Field<float>("Amount"),
                                     BookingStatus = dataRow.Field<int>("BookingStatus"),
                                     FSMDetail = dataRow.Field<int?>("FSMDetailID").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.FSMDetail { ID = dataRow.Field<int>("FSMDetailID"), Name = dataRow.Field<String>("FSMDetailName"), PhoneNumber = dataRow.Field<String>("FSMDetailPhoneNumber") },
                                     Office = new DataModel.Modal.Office { OfficeId = dataRow.Field<int>("OfficeId"), Name = dataRow.Field<String>("OfficeName") },
                                     Event = dataRow.Field<int?>("EventId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.Event { EventId = dataRow.Field<int>("EventId"), Name = dataRow.Field<String>("EventName") },
                                     Convention = dataRow.Field<int?>("ConventionId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.Convention { ConventionId = dataRow.Field<int>("ConventionId"), Name = dataRow.Field<String>("ConventionName") },
                                     VisitType = dataRow.Field<int?>("VisitTypeId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.VisitType { VisitTypeId = dataRow.Field<int>("VisitTypeId"), VisitTypeName = dataRow.Field<String>("VisitTypeName") },
                                     Service = dataRow.Field<int?>("ServiceId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.Service { ServiceId = dataRow.Field<int>("ServiceId"), ServiceName = dataRow.Field<String>("ServiceName") }
                                 }).ToList();
                return audiences;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DataModel.Modal.Audience> GetAudiences(int conventionID)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetAudienceByConventionID]"))
                {
                    database.AddInParameter(command, "@ConventionID", DbType.Int32, conventionID);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var audiences = (from dataRow in dataTable.AsEnumerable()
                                 select new DataModel.Modal.Audience
                                 {
                                     AudienceID = dataRow.Field<int>("AudienceID"),
                                     Name = dataRow.Field<String>("Name"),
                                     Contact = dataRow.Field<String>("Contact"),
                                     VisitDate = dataRow.Field<DateTime>("VisitDate"),
                                     GSBAmount = dataRow.Field<float>("GSBAmount"),
                                     IsAttended = dataRow.Field<bool>("IsAttended"),
                                     FSMName = dataRow.Field<String>("FSMName"),
                                     Amount = dataRow.Field<float>("Amount"),
                                     BookingStatus = dataRow.Field<int>("BookingStatus"),
                                     Office = dataRow.Field<int?>("OfficeId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.Office { OfficeId = dataRow.Field<int>("OfficeId"), Name = dataRow.Field<String>("OfficeName") },
                                     Event = dataRow.Field<int?>("EventId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.Event { EventId = dataRow.Field<int>("EventId"), Name = dataRow.Field<String>("EventName") },
                                     Convention = dataRow.Field<int?>("ConventionId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.Convention { ConventionId = dataRow.Field<int>("ConventionId"), Name = dataRow.Field<String>("ConventionName") },
                                     VisitType = dataRow.Field<int?>("VisitTypeId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.VisitType { VisitTypeId = dataRow.Field<int>("VisitTypeId"), VisitTypeName = dataRow.Field<String>("VisitTypeName") }
                                 }).ToList();
                return audiences;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DataModel.Modal.Audience> GetAudiencesByEventID(int eventID)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetAudienceByEventID]"))
                {
                    database.AddInParameter(command, "@eventID", DbType.Int32, eventID);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var audiences = new List<DataModel.Modal.Audience>();
                audiences = (from dataRow in dataTable.AsEnumerable()
                             select new DataModel.Modal.Audience
                             {
                                 AudienceID = dataRow.Field<int>("AudienceID"),
                                 Name = dataRow.Field<String>("Name"),
                                 Contact = dataRow.Field<String>("Contact"),
                                 VisitDate = dataRow.Field<DateTime>("VisitDate"),
                                 GSBAmount = dataRow.Field<float>("GSBAmount"),
                                 IsAttended = dataRow.Field<bool>("IsAttended"),
                                 FSMName = dataRow.Field<String>("FSMName"),
                                 Amount = dataRow.Field<float>("Amount"),
                                 BookingStatus = dataRow.Field<int>("BookingStatus"),
                                 Office = new DataModel.Modal.Office { OfficeId = dataRow.Field<int>("OfficeId"), Name = dataRow.Field<String>("OfficeName") },
                                 Event = dataRow.Field<int?>("EventId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.Event { EventId = dataRow.Field<int>("EventId"), Name = dataRow.Field<String>("EventName") },
                                 Convention = dataRow.Field<int?>("ConventionId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.Convention { ConventionId = dataRow.Field<int>("ConventionId"), Name = dataRow.Field<String>("ConventionName") },
                                 VisitType = dataRow.Field<int?>("VisitTypeId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.VisitType { VisitTypeId = dataRow.Field<int>("VisitTypeId"), VisitTypeName = dataRow.Field<String>("VisitTypeName") }
                             }).ToList();
                return audiences;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataModel.Modal.ChartSeries GetFundingTargetsAchived(List<int> officeIDs, DateTime startDate, DateTime endDate)
        {
            try
            {
                DataSet dataSet;
                var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_FundingTargetsAchivedByOfficeIDs]"))
                {
                    database.AddInParameter(command, "@IDs", DbType.String, string.Join("|", officeIDs.ToArray()));
                    database.AddInParameter(command, "@StartDate", DbType.DateTime, startDate);
                    database.AddInParameter(command, "@EndDate", DbType.DateTime, endDate);
                    dataSet = database.ExecuteDataSet(command);
                }
                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var targetSeriesData = (from dataRow in dataTable.AsEnumerable()
                                        select new DataModel.Modal.ChartSeries.DataPoint
                                        {
                                            weekNumber = dataRow.Field<int>("WeekNumber"),
                                            x = (dataRow.Field<DateTime>("WeekStartDate").AddDays(6) - startYear).TotalMilliseconds,
                                            y = dataRow.Field<double>("FundRaised")
                                        }).ToList();
                var totalTarget = targetSeriesData.Sum(model => model.y);
                return new DataModel.Modal.ChartSeries { type = "line", name = "Achived Tagert Year - " + DateTime.Now.Year, data = targetSeriesData, };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataModel.Modal.ChartSeries GetGSBTargetsAchived(List<int> officeIDs, DateTime startDate, DateTime endDate)
        {
            try
            {
                DataSet dataSet;
                var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GSBTargetsAchivedByOfficeIDs]"))
                {
                    database.AddInParameter(command, "@IDs", DbType.String, string.Join("|", officeIDs.ToArray()));
                    database.AddInParameter(command, "@StartDate", DbType.DateTime, startDate);
                    database.AddInParameter(command, "@EndDate", DbType.DateTime, endDate);
                    dataSet = database.ExecuteDataSet(command);
                }
                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var targetSeriesData = (from dataRow in dataTable.AsEnumerable()
                                        select new DataModel.Modal.ChartSeries.DataPoint
                                        {
                                            weekNumber = dataRow.Field<int>("WeekNumber"),
                                            x = (dataRow.Field<DateTime>("WeekStartDate").AddDays(6) - startYear).TotalMilliseconds,
                                            y = dataRow.Field<double>("GSBAmount")
                                        }).ToList();
                var totalTarget = targetSeriesData.Sum(model => model.y);
                return new DataModel.Modal.ChartSeries { type = "line", name = "Achived Tagert Year - " + DateTime.Now.Year, data = targetSeriesData, };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataModel.Modal.ChartSeries GetBookingTargetsAchived(List<int> officeIDs, DateTime startDate, DateTime endDate)
        {
            try
            {
                DataSet dataSet;
                var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_BookingTargetsAchivedByOfficeIDs]"))
                {
                    database.AddInParameter(command, "@IDs", DbType.String, string.Join("|", officeIDs.ToArray()));
                    database.AddInParameter(command, "@StartDate", DbType.DateTime, startDate);
                    database.AddInParameter(command, "@EndDate", DbType.DateTime, endDate);
                    dataSet = database.ExecuteDataSet(command);
                }
                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var targetSeriesData = (from dataRow in dataTable.AsEnumerable()
                                        select new DataModel.Modal.ChartSeries.DataPoint
                                        {
                                            weekNumber = dataRow.Field<int>("WeekNumber"),
                                            x = (dataRow.Field<DateTime>("WeekStartDate").AddDays(6) - startYear).TotalMilliseconds,
                                            y = dataRow.Field<Int32>("Booking")
                                        }).ToList();
                var totalTarget = targetSeriesData.Sum(model => model.y);
                return new DataModel.Modal.ChartSeries { type = "line", name = "Achived Tagert Year - " + DateTime.Now.Year, data = targetSeriesData, };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataModel.Modal.ChartSeries GetArrivalTargetsAchived(List<int> officeIDs, DateTime startDate, DateTime endDate)
        {
            try
            {
                DataSet dataSet;
                var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_ArrivalTargetsAchivedByOfficeIDs]"))
                {
                    database.AddInParameter(command, "@IDs", DbType.String, string.Join("|", officeIDs.ToArray()));
                    database.AddInParameter(command, "@StartDate", DbType.DateTime, startDate);
                    database.AddInParameter(command, "@EndDate", DbType.DateTime, endDate);
                    dataSet = database.ExecuteDataSet(command);
                }
                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var targetSeriesData = (from dataRow in dataTable.AsEnumerable()
                                        select new DataModel.Modal.ChartSeries.DataPoint
                                        {
                                            weekNumber = dataRow.Field<int>("WeekNumber"),
                                            x = (dataRow.Field<DateTime>("WeekStartDate").AddDays(6) - startYear).TotalMilliseconds,
                                            y = dataRow.Field<Int32>("Arrival")
                                        }).ToList();
                var totalTarget = targetSeriesData.Sum(model => model.y);
                return new DataModel.Modal.ChartSeries { type = "line", name = "Achived Tagert Year - " + DateTime.Now.Year, data = targetSeriesData, };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DataModel.Modal.Audience> GetArrivalAudiences(int officeID, DateTime startDate, DateTime endDate)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetArrivalAudienceByOfficeID]"))
                {
                    database.AddInParameter(command, "@OfficeID", DbType.Int32, officeID);
                    database.AddInParameter(command, "@StartDate", DbType.DateTime, startDate);
                    database.AddInParameter(command, "@EndDate", DbType.DateTime, endDate);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var audiences = (from dataRow in dataTable.AsEnumerable()
                                 select new DataModel.Modal.Audience
                                 {
                                     AudienceID = dataRow.Field<int>("AudienceID"),
                                     Name = dataRow.Field<String>("Name"),
                                     //Contact = dataRow.Field<String>("Contact"),
                                     //VisitDate = dataRow.Field<DateTime>("VisitDate"),
                                     //GSBAmount = dataRow.Field<float>("GSBAmount"),
                                     //IsAttended = dataRow.Field<bool>("IsAttended"),
                                     //FSMName = dataRow.Field<String>("FSMName"),
                                     //Amount = dataRow.Field<float>("Amount"),
                                     //BookingStatus = dataRow.Field<int>("BookingStatus"),
                                     //Office = new DataModel.Modal.Office { OfficeId = dataRow.Field<int>("OfficeId"), Name = dataRow.Field<String>("OfficeName") },
                                     //Event = dataRow.Field<int?>("EventId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.Event { EventId = dataRow.Field<int>("EventId"), Name = dataRow.Field<String>("EventName") },
                                     Convention = dataRow.Field<int?>("ConventionId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.Convention { ConventionId = dataRow.Field<int>("ConventionId"), Name = dataRow.Field<String>("ConventionName"), StartDate = dataRow.Field<DateTime>("StartDate") },
                                     //VisitType = dataRow.Field<int?>("VisitTypeId").GetValueOrDefault(0) == 0 ? null : new DataModel.Modal.VisitType { VisitTypeId = dataRow.Field<int>("VisitTypeId"), VisitTypeName = dataRow.Field<String>("VisitTypeName") }
                                 }).ToList();
                return audiences;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}