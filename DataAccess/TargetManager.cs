
#region Using Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace DataAccess
{
    public class TargetManager : DBManager
    {
        public bool Add(int officeID, DateTime dueDate, int bookingTargets, float fundRaisingAmount, float gsbAmount, int arrivalTargets)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_AddTarget]"))
                {
                    database.AddInParameter(command, "@Booking", DbType.Int32, bookingTargets);
                    database.AddInParameter(command, "@FundRaising", DbType.Single, fundRaisingAmount);
                    database.AddInParameter(command, "@GSB", DbType.Single, gsbAmount);
                    database.AddInParameter(command, "@Arrivals", DbType.Int32, arrivalTargets);
                    database.AddInParameter(command, "@DueDate", DbType.DateTime, dueDate);
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

        public bool Update(int targetID, int officeID, DateTime dueDate, int bookingTargets, float fundRaisingAmount, float gsbAmount, int arrivalTargets)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_UpdateTarget]"))
                {
                    database.AddInParameter(command, "@ID", DbType.Int32, targetID);
                    database.AddInParameter(command, "@Booking", DbType.Int32, bookingTargets);
                    database.AddInParameter(command, "@FundRaising", DbType.Single, fundRaisingAmount);
                    database.AddInParameter(command, "@GSB", DbType.Single, gsbAmount);
                    database.AddInParameter(command, "@Arrivals", DbType.Int32, arrivalTargets);
                    database.AddInParameter(command, "@DueDate", DbType.DateTime, dueDate);
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

        public bool Delete(int id)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_DeletTarget]"))
                {
                    database.AddInParameter(command, "@ID", DbType.Int32, id);
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

        public List<DataModel.Modal.Target> GetTargets()
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetTargets]"))
                {
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var targets = (from dataRow in dataTable.AsEnumerable()
                               select new DataModel.Modal.Target
                               {
                                   TargetId = dataRow.Field<int>("TargetId"),
                                   Booking = dataRow.Field<int>("Booking"),
                                   FundRaising = dataRow.Field<float>("FundRaising"),
                                   GSB = dataRow.Field<float>("GSB"),
                                   Arrivals = dataRow.Field<float>("Arrivals"),
                                   DueDate = dataRow.Field<DateTime>("DueDate"),
                                   CreateDate = dataRow.Field<DateTime>("CreateDate"),
                                   UpdateDate = dataRow.Field<DateTime>("UpdateDate"),
                                   Office = new DataModel.Modal.Office { OfficeId = dataRow.Field<int>("OfficeId"), Name = dataRow.Field<string>("OfficeName") }

                               }).ToList();
                return targets;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataModel.Modal.Target GetTarget(int id)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetTargetByID]"))
                {
                    database.AddInParameter(command, "@ID", DbType.Int32, id);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var target = (from dataRow in dataTable.AsEnumerable()
                              select new DataModel.Modal.Target
                              {
                                  TargetId = dataRow.Field<int>("TargetId"),
                                  Booking = dataRow.Field<int>("Booking"),
                                  FundRaising = dataRow.Field<float>("FundRaising"),
                                  GSB = dataRow.Field<float>("GSB"),
                                  Arrivals = dataRow.Field<float>("Arrivals"),
                                  DueDate = dataRow.Field<DateTime>("DueDate"),
                                  CreateDate = dataRow.Field<DateTime>("CreateDate"),
                                  UpdateDate = dataRow.Field<DateTime>("UpdateDate"),
                                  Office = new DataModel.Modal.Office { OfficeId = dataRow.Field<int>("OfficeId") }
                              }).FirstOrDefault();
                return target;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DataModel.Modal.Target> GetTargets(List<int> officeIDs)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetTargetsByOfficeIDs]"))
                {
                    database.AddInParameter(command, "@OfficeIDs", DbType.String, string.Join("|", officeIDs.ToArray()));
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var targets = (from dataRow in dataTable.AsEnumerable()
                               select new DataModel.Modal.Target
                               {
                                   TargetId = dataRow.Field<int>("TargetId"),
                                   Booking = dataRow.Field<int>("Booking"),
                                   FundRaising = dataRow.Field<float>("FundRaising"),
                                   GSB = dataRow.Field<float>("GSB"),
                                   Arrivals = dataRow.Field<float>("Arrivals"),
                                   DueDate = dataRow.Field<DateTime>("DueDate"),
                                   CreateDate = dataRow.Field<DateTime>("CreateDate"),
                                   UpdateDate = dataRow.Field<DateTime>("UpdateDate"),
                                   Office = new DataModel.Modal.Office { OfficeId = dataRow.Field<int>("OfficeId"), Name = dataRow.Field<string>("OfficeName") }
                               }).ToList();
                return targets;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataModel.Modal.ChartSeries GetFundingTargets(List<int> officeIDs, DateTime startDate, DateTime endDate)
        {
            try
            {
                DataSet dataSet;
                var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetFundingTargetsByOfficeIDs]"))
                {
                    database.AddInParameter(command, "@OfficeIDs", DbType.String, string.Join("|", officeIDs.ToArray()));
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
                                            y = dataRow.Field<double>("FundRaising")
                                        }).ToList();
                var totalTarget = targetSeriesData.Sum(model => model.y);
                return new DataModel.Modal.ChartSeries { type = "line", name = "Tagert Year - " + DateTime.Now.Year, data = targetSeriesData, };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataModel.Modal.ChartSeries GetBookingTargets(List<int> officeIDs, DateTime startDate, DateTime endDate)
        {
            try
            {
                DataSet dataSet;
                var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetBookingTargetsByOfficeIDs]"))
                {
                    database.AddInParameter(command, "@OfficeIDs", DbType.String, string.Join("|", officeIDs.ToArray()));
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
                                            y = dataRow.Field<int>("Booking")
                                        }).ToList();
                var totalTarget = targetSeriesData.Sum(model => model.y);
                return new DataModel.Modal.ChartSeries { type = "line", name = "Tagert Year - " + DateTime.Now.Year, data = targetSeriesData, };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataModel.Modal.ChartSeries GetGSBTargets(List<int> officeIDs, DateTime startDate, DateTime endDate)
        {
            try
            {
                DataSet dataSet;
                var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetGSBTargetsByOfficeIDs]"))
                {
                    database.AddInParameter(command, "@OfficeIDs", DbType.String, string.Join("|", officeIDs.ToArray()));
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
                                            y = dataRow.Field<double>("GSB")
                                        }).ToList();
                var totalTarget = targetSeriesData.Sum(model => model.y);
                return new DataModel.Modal.ChartSeries { type = "line", name = "Tagert Year - " + DateTime.Now.Year, data = targetSeriesData, };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataModel.Modal.ChartSeries GetArrivalTargets(List<int> officeIDs, DateTime startDate, DateTime endDate)
        {
            try
            {
                DataSet dataSet;
                var startYear = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetArrivalTargetsByOfficeIDs]"))
                {
                    database.AddInParameter(command, "@OfficeIDs", DbType.String, string.Join("|", officeIDs.ToArray()));
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
                                            y = dataRow.Field<double>("Arrivals")
                                        }).ToList();
                var totalTarget = targetSeriesData.Sum(model => model.y);
                return new DataModel.Modal.ChartSeries { type = "line", name = "Tagert Year - " + DateTime.Now.Year, data = targetSeriesData, };
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
