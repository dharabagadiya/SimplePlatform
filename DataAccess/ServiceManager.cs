
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
    }
}
