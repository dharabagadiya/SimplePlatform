
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
    public class CommentManager : DBManager
    {
        public bool Add(int taskID, int userID, string message, List<DataModel.Modal.CommentAttachment> commentAttachments)
        {
            int retunvalue;
            try
            {
                var datatable = new DataTable();
                datatable.Columns.Add("Name", typeof(string));
                datatable.Columns.Add("Path", typeof(string));
                if (commentAttachments != null)
                {
                    foreach (var commentAttachment in commentAttachments)
                    {
                        var row = datatable.NewRow();
                        row["Name"] = commentAttachment.FileResource.name;
                        row["Path"] = commentAttachment.FileResource.path;
                        datatable.Rows.Add(row);
                    }
                }
                using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-7QFA5C9\\MSSQLSERVER_2012;Initial Catalog=SimplePlatformTemp;user id=sa;password=sa;"))
                {
                    con.Open();
                    using (var command = new SqlCommand())
                    {
                        command.Connection = con;
                        command.CommandText = "[dbo].[sproc_SimplePlatForm_AddComment]";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter { ParameterName = "@taksID", SqlDbType = SqlDbType.Int, Value = taskID });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@userID", SqlDbType = SqlDbType.Int, Value = userID });
                        command.Parameters.Add(new SqlParameter { ParameterName = "@message", SqlDbType = SqlDbType.VarChar, Value = message });
                        var dataTableParameter = command.Parameters.AddWithValue("@Attachments", datatable.GetChanges());
                        dataTableParameter.SqlDbType = SqlDbType.Structured;
                        dataTableParameter.TypeName = "[dbo].[Attachments]";
                        command.Parameters.Add(new SqlParameter { ParameterName = "@Status", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.ReturnValue });
                        command.ExecuteNonQuery();
                        retunvalue = (int)command.Parameters["@Status"].Value;
                    }
                    con.Close();
                }
                return retunvalue == 1;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<DataModel.Modal.CommentAttachment> GetCommentAttachment(int id)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetCommentAttachmentByID]"))
                {
                    database.AddInParameter(command, "@ID", DbType.Int32, id);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var commentAttachments = (from dataRow in dataTable.AsEnumerable()
                                          select new DataModel.Modal.CommentAttachment
                                          {
                                              FileResource = new DataModel.Modal.FileResource
                                              {
                                                  Id = dataRow.Field<int>("FileResourceID"),
                                                  name = dataRow.Field<string>("Name"),
                                                  path = dataRow.Field<string>("Path")
                                              }
                                          }).ToList();
                return commentAttachments;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DataModel.Modal.Comment> GetComments(int taskID)
        {
            try
            {
                DataSet dataSet;
                using (var command = database.GetStoredProcCommand("[dbo].[sproc_SimplePlatForm_GetCommentsByTaskID]"))
                {
                    database.AddInParameter(command, "@TaskID", DbType.Int32, taskID);
                    dataSet = database.ExecuteDataSet(command);
                }

                if (dataSet == null || dataSet.Tables.Count <= 0) return null;
                var dataTable = dataSet.Tables[0];
                var comments = (from dataRow in dataTable.AsEnumerable()
                                select new DataModel.Modal.Comment
                                {
                                    CommentId = dataRow.Field<int>("CommentId"),
                                    CommentText = dataRow.Field<string>("CommentText"),
                                    CreateDate = dataRow.Field<DateTime>("CreateDate"),
                                    UpdateDate = dataRow.Field<DateTime>("UpdateDate"),
                                    UserDetail = new DataModel.Modal.UserDetail
                                    {
                                        UserId = dataRow.Field<int>("UserId"),
                                        User = new CustomAuthentication.User
                                        {
                                            UserId = dataRow.Field<int>("UserId"),
                                            FirstName = dataRow.Field<string>("FirstName"),
                                            LastName = dataRow.Field<string>("LastName")
                                        }
                                    },
                                    IsFileAttached = dataRow.Field<int>("IsFileAttached") != 0
                                }).ToList();
                return comments;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}