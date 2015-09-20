using System;

#region Using Namespaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace DataAccess
{
    public class VisitTypeManager : DBManager
    {
        public List<DataModel.Modal.VisitType> GetVisitTypes()
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetVisitTypes]"))
                {
                    dataSet = database.ExecuteDataSet(command);
                }
                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var visitTypes = (from dataRow in dataTable.AsEnumerable()
                                  select new DataModel.Modal.VisitType
                                  {
                                      VisitTypeId = dataRow.Field<int>("VisitTypeId"),
                                      VisitTypeName = dataRow.Field<string>("VisitTypeName")
                                  }).ToList();
                return visitTypes;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
