
#region Using Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
#endregion

namespace CustomAuthentication
{
    public class CustomRoleProvider : DataAccess.DBManager
    {
        public List<Role> GetAllRoles()
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetRoles]"))
                {
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var roles = (from dataRow in dataTable.AsEnumerable()
                             select new Role
                             {
                                 RoleId = dataRow.Field<int>("RoleId"),
                                 RoleName = dataRow.Field<string>("RoleName")
                             }).ToList();
                return roles;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Role GetRole(string roleName)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetRoleByName]"))
                {
                    database.AddInParameter(command, "@roleName", DbType.String, roleName);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var role = (from dataRow in dataTable.AsEnumerable()
                            select new Role
                            {
                                RoleId = dataRow.Field<int>("RoleId"),
                                RoleName = dataRow.Field<string>("RoleName")
                            }).FirstOrDefault();
                return role;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}