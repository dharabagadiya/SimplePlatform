
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OfficeMananer : DBManager
    {
        public bool Add(string name, string contactNo, string city, List<string> userID, string path, string fileName)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_AddOffice]"))
                {
                    database.AddInParameter(command, "@name", DbType.String, name);
                    database.AddInParameter(command, "@contactNo", DbType.String, contactNo);
                    database.AddInParameter(command, "@city", DbType.String, city);
                    database.AddInParameter(command, "@userID", DbType.String, string.Join("|", userID));
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

        public bool Add(string name, string contactNo, string city, List<string> userID)
        {
            return Add(name, contactNo, city, userID, string.Empty, string.Empty);
        }

        public bool Update(int id, string name, string contactNo, string city, List<string> userID, string path, string fileName)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_UpdateOffice]"))
                {
                    database.AddInParameter(command, "@officeID", DbType.Int32, id);
                    database.AddInParameter(command, "@name", DbType.String, name);
                    database.AddInParameter(command, "@contactNo", DbType.String, contactNo);
                    database.AddInParameter(command, "@city", DbType.String, city);
                    database.AddInParameter(command, "@userID", DbType.String, string.Join("|", userID));
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

        public bool Update(int id, string name, string contactNo, string city, List<string> userID)
        {
            return Update(id, name, contactNo, city, userID, string.Empty, string.Empty);
        }

        public bool Delete(int id)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_DeleteOffice]"))
                {
                    database.AddInParameter(command, "@officeID", DbType.Int32, id);
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

        public List<DataModel.Modal.Office> GetOffices(int userID)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetOfficesByUserID]"))
                {
                    database.AddInParameter(command, "@userID", DbType.Int32, userID);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var offices = (from dataRow in dataTable.AsEnumerable()
                               select new DataModel.Modal.Office
                               {
                                   OfficeId = dataRow.Field<int>("OfficeId"),
                                   Name = dataRow.Field<string>("Name"),
                                   ContactNo = dataRow.Field<string>("ContactNo"),
                                   City = dataRow.Field<string>("City"),
                                   FileResource = new DataModel.Modal.FileResource { Id = dataRow.Field<int>("Id"), name = dataRow.Field<string>("name"), path = dataRow.Field<string>("path") }
                               }).ToList();
                return offices;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataModel.Modal.Office GetOffice(int id)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetOfficesByID]"))
                {
                    database.AddInParameter(command, "@ID", DbType.Int32, id);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var userManager = new DataAccess.UserManager();
                var office = (from dataRow in dataTable.AsEnumerable()
                              select new DataModel.Modal.Office
                              {
                                  OfficeId = dataRow.Field<int>("OfficeId"),
                                  Name = dataRow.Field<string>("Name"),
                                  ContactNo = dataRow.Field<string>("ContactNo"),
                                  City = dataRow.Field<string>("City"),
                                  UsersDetail = userManager.GetUserByOfficeID(dataRow.Field<int>("OfficeId")),
                                  FileResource = new DataModel.Modal.FileResource { Id = dataRow.Field<int>("Id"), name = dataRow.Field<string>("name"), path = dataRow.Field<string>("path") }
                              }).FirstOrDefault();
                return office;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
