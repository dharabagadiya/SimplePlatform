
#region Using Namespaces
using DataModel.Modal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
#endregion

namespace DataAccess
{
    public class FSMDetailManager : DBManager
    {
        public List<FSMDetail> FSMDetails()
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetFSMDetails]"))
                {
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var fsmDetails = new List<FSMDetail>();
                fsmDetails = (from dataRow in dataTable.AsEnumerable()
                              select new DataModel.Modal.FSMDetail
                              {
                                  ID = dataRow.Field<int>("Id"),
                                  Name = dataRow.Field<string>("Name"),
                                  PhoneNumber = dataRow.Field<string>("PhoneNumber")
                              }).ToList();
                return fsmDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int Add(string name, string emailID, string phoneNumber)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_AddFSMDetail]"))
                {
                    database.AddInParameter(command, "@name", DbType.String, name);
                    database.AddInParameter(command, "@emailAddress", DbType.String, emailID);
                    database.AddInParameter(command, "@phoneNumber", DbType.String, phoneNumber);
                    database.AddOutParameter(command, "@Status", DbType.Int32, returnVale);
                    database.ExecuteNonQuery(command);
                    returnVale = (int)database.GetParameterValue(command, "@Status");
                }
                return returnVale;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
