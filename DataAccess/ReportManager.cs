
#region Using Namespaces
using DataModel.Modal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
#endregion

namespace DataAccess
{
    public class ReportManager : DBManager
    {
        public DataSet GetArrivalAudiences(List<int> officeIDs, DateTime startDate, DateTime endDate)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetArrivalsByOfficeIDs]"))
                {
                    database.AddInParameter(command, "@IDs", DbType.String, string.Join("|", officeIDs.ToArray()));
                    database.AddInParameter(command, "@StartDate", DbType.DateTime, startDate);
                    database.AddInParameter(command, "@EndDate", DbType.DateTime, endDate);
                    dataSet = database.ExecuteDataSet(command);
                }
                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                return dataSet;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
