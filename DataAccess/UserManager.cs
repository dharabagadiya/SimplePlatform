using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Modal;
using CustomAuthentication;
using System.Data;

namespace DataAccess
{
    public class UserManager : DBManager
    {
        public List<UserDetail> GetUsersByRoleID(int roleID)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetUserByRoleID]"))
                {
                    database.AddInParameter(command, "@RoleID", DbType.Int32, roleID);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var userDetails = (from dataRow in dataTable.AsEnumerable()
                                   select new DataModel.Modal.UserDetail
                                   {
                                       UserId = dataRow.Field<int>("UserId"),
                                       User = new User
                                       {
                                           UserId = dataRow.Field<int>("UserId"),
                                           UserName = dataRow.Field<string>("UserName"),
                                           FirstName = dataRow.Field<string>("FirstName"),
                                           LastName = dataRow.Field<string>("LastName"),
                                           Email = dataRow.Field<string>("Email"),
                                           Roles = new List<Role> {
                                            new Role {
                                                RoleId = dataRow.Field<int>("RoleId"),
                                                RoleName = dataRow.Field<string>("RoleName")
                                            }
                                        }
                                       },
                                       FileResource = dataRow.Field<int?>("FileResourceID").GetValueOrDefault(0) == 0 ? null : new FileResource
                                       {
                                           Id = dataRow.Field<int>("FileResourceID"),
                                           name = dataRow.Field<string>("FileResourceName"),
                                           path = dataRow.Field<string>("FileResourcePath")
                                       }
                                   }).ToList();
                return userDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public UserDetail GetUserDetail(int id)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetUserByID]"))
                {
                    database.AddInParameter(command, "@ID", DbType.Int32, id);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var officeManager = new DataAccess.OfficeMananer();
                var dataTable = dataSet.Tables[0];
                var userDetail = (from dataRow in dataTable.AsEnumerable()
                                  select new DataModel.Modal.UserDetail
                                  {
                                      UserId = dataRow.Field<int>("UserId"),
                                      User = new User
                                      {
                                          UserId = dataRow.Field<int>("UserId"),
                                          UserName = dataRow.Field<string>("UserName"),
                                          FirstName = dataRow.Field<string>("FirstName"),
                                          LastName = dataRow.Field<string>("LastName"),
                                          Email = dataRow.Field<string>("Email"),
                                          Roles = new List<Role> {
                                            new Role {
                                                RoleId = dataRow.Field<int>("RoleId"),
                                                RoleName = dataRow.Field<string>("RoleName")
                                            }
                                        }
                                      },
                                      Offices = officeManager.GetOffices(dataRow.Field<int>("UserId")),
                                      FileResource = dataRow.Field<int?>("FileResourceID").GetValueOrDefault(0) == 0 ? null : new FileResource
                                      {
                                          Id = dataRow.Field<int>("FileResourceID"),
                                          name = dataRow.Field<string>("FileResourceName"),
                                          path = dataRow.Field<string>("FileResourcePath")
                                      }
                                  }).FirstOrDefault();
                return userDetail;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool AddDateDuration(DateTime startDate, DateTime endDate, int userID)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_UpdateUserDateDuration]"))
                {
                    database.AddInParameter(command, "@UserID", DbType.Int32, userID);
                    database.AddInParameter(command, "@StartDate", DbType.DateTime, startDate);
                    database.AddInParameter(command, "@EndDate", DbType.DateTime, endDate);
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
        public bool UpdatePassword(string oldPassword, string newPassword, int userID)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_UpdateUserPassword]"))
                {
                    database.AddInParameter(command, "@UserID", DbType.Int32, userID);
                    database.AddInParameter(command, "@OldPassword", DbType.DateTime, oldPassword);
                    database.AddInParameter(command, "@NewPassword", DbType.DateTime, newPassword);
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
        public bool DeleteUser(int id)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_DeleteUserByID]"))
                {
                    database.AddInParameter(command, "@UserID", DbType.Int32, id);
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
        public bool UpdateUser(int id, string firstName, string lastName, string emildID, int userRoleID, List<string> officesID, string fileName, string path)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_UpdateUserByID]"))
                {
                    database.AddInParameter(command, "@UserID", DbType.Int32, id);
                    database.AddInParameter(command, "@firstName", DbType.String, firstName);
                    database.AddInParameter(command, "@lastName", DbType.String, lastName);
                    database.AddInParameter(command, "@emildID", DbType.String, emildID);
                    database.AddInParameter(command, "@userRoleID", DbType.Int32, userRoleID);
                    database.AddInParameter(command, "@officeID", DbType.String, string.Join("|", officesID));
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
        public bool UpdateUser(int id, string firstName, string lastName, string emildID, int userRoleID, List<string> officesID)
        {
            return UpdateUser(id, firstName, lastName, emildID, userRoleID, officesID, string.Empty, string.Empty);
        }
        public bool CreateUser(string firstName, string lastName, string emildID, int userRoleID, List<string> officeID)
        {
            try
            {
                var returnVale = 0;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_CreateUser]"))
                {
                    database.AddInParameter(command, "@firstName", DbType.String, firstName);
                    database.AddInParameter(command, "@lastName", DbType.String, lastName);
                    database.AddInParameter(command, "@emildID", DbType.String, emildID);
                    database.AddInParameter(command, "@userRoleID", DbType.Int32, userRoleID);
                    database.AddInParameter(command, "@officeID", DbType.String, string.Join("|", officeID.ToArray()));
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
        public List<UserDetail> GetUsers(int userID)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetUsers]"))
                {
                    database.AddInParameter(command, "@UserID", DbType.Int32, userID);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var officeMananer = new OfficeMananer();
                var userDetails = (from dataRow in dataTable.AsEnumerable()
                                   select new DataModel.Modal.UserDetail
                                   {
                                       UserId = dataRow.Field<int>("UserId"),
                                       User = new User
                                       {
                                           UserId = dataRow.Field<int>("UserId"),
                                           UserName = dataRow.Field<string>("UserName"),
                                           FirstName = dataRow.Field<string>("FirstName"),
                                           LastName = dataRow.Field<string>("LastName"),
                                           Email = dataRow.Field<string>("Email"),
                                           CreateDate = dataRow.Field<DateTime>("CreateDate"),
                                           Roles = new List<Role> {
                                            new Role {
                                                RoleId = dataRow.Field<int>("RoleId"),
                                                RoleName = dataRow.Field<string>("RoleName")
                                            }
                                        }
                                       },
                                       Offices = officeMananer.GetOffices(dataRow.Field<int>("UserId")),
                                       FileResource = dataRow.Field<int?>("FileResourceID").GetValueOrDefault(0) == 0 ? null : new FileResource
                                       {
                                           Id = dataRow.Field<int>("FileResourceID"),
                                           name = dataRow.Field<string>("FileResourceName"),
                                           path = dataRow.Field<string>("FileResourcePath")
                                       }
                                   }).ToList();
                return userDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<UserDetail> GetUserByOfficeID(int officeID, int roleID)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetUserByOfficeID]"))
                {
                    database.AddInParameter(command, "@OfficeID", DbType.Int32, officeID);

                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var officeMananer = new OfficeMananer();
                var userDetails = new List<UserDetail>();
                userDetails = (from dataRow in dataTable.AsEnumerable()
                               select new DataModel.Modal.UserDetail
                               {
                                   UserId = dataRow.Field<int>("UserId"),
                                   User = new User
                                   {
                                       UserId = dataRow.Field<int>("UserId"),
                                       UserName = dataRow.Field<string>("UserName"),
                                       FirstName = dataRow.Field<string>("FirstName"),
                                       LastName = dataRow.Field<string>("LastName"),
                                       Email = dataRow.Field<string>("Email"),
                                       Roles = new List<Role> {
                                            new Role {
                                                RoleId = dataRow.Field<int>("RoleId"),
                                                RoleName = dataRow.Field<string>("RoleName")
                                            }
                                        }
                                   },
                                   FileResource = dataRow.Field<int?>("FileResourceID").GetValueOrDefault(0) == 0 ? null : new FileResource
                                   {
                                       Id = dataRow.Field<int>("FileResourceID"),
                                       name = dataRow.Field<string>("FileResourceName"),
                                       path = dataRow.Field<string>("FileResourcePath")
                                   }
                               }).ToList();
                return userDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
