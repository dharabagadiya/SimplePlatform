
#region Using Namespaces
using DataModel.Modal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
#endregion

namespace DataAccess
{
    public class ServiceManager : DBManager
    {
        public List<Service> GetServices()
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetServices]"))
                {
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var services = new List<Service>();
                services = (from dataRow in dataTable.AsEnumerable()
                            select new DataModel.Modal.Service
                            {
                                ServiceId = dataRow.Field<int>("ServiceId"),
                                ServiceName = dataRow.Field<string>("ServiceName")
                            }).ToList();
                return services;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Service GetService(int id)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetServiceByID]"))
                {
                    database.AddInParameter(command, "@ID", DbType.Int32, id);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var service = (from dataRow in dataTable.AsEnumerable()
                               select new DataModel.Modal.Service
                               {
                                   ServiceId = dataRow.Field<int>("ServiceId"),
                                   ServiceName = dataRow.Field<string>("ServiceName")
                               }).FirstOrDefault();
                return service;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Add(string name)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_AddService]"))
                {
                    database.AddInParameter(command, "@name", DbType.String, name);
                    database.AddOutParameter(command, "@Status", DbType.Int32, returnVale);
                    database.ExecuteNonQuery(command);
                    returnVale = (int)database.GetParameterValue(command, "@Status");
                }
                return returnVale != 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(int id, string name)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_UpdateService]"))
                {
                    database.AddInParameter(command, "@ID", DbType.Int32, id);
                    database.AddInParameter(command, "@name", DbType.String, name);
                    database.AddOutParameter(command, "@Status", DbType.Int32, returnVale);
                    database.ExecuteNonQuery(command);
                    returnVale = (int)database.GetParameterValue(command, "@Status");
                }
                return returnVale != 0;
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
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_DeleteService]"))
                {
                    database.AddInParameter(command, "@ID", DbType.Int32, id);
                    database.AddOutParameter(command, "@Status", DbType.Int32, returnVale);
                    database.ExecuteNonQuery(command);
                    returnVale = (int)database.GetParameterValue(command, "@Status");
                }
                return returnVale != 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
