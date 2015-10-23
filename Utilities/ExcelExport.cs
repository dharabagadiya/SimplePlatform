
#region Using Namspaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace Utilities
{
    public class ExcelExport
    {
        public class ColumnDetail
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string Type { get; set; }
            public string Style { get; set; }
            public string TitleStyle { get; set; }
        }

        public static string ExportExcel(string sheetName, List<ColumnDetail> columnMappings, DataTable dataTable)
        {
            var sb = new StringBuilder("<root>");
            sb.AppendFormat("<sheet name = 'Sheet1' cellstylemode = 'name' renameto='{0}'>", sheetName);

            #region Table Detail
            sb.Append("<region startat='a1'>");

            #region Table Title
            sb.Append("<r>");
            foreach (var columnMapping in columnMappings)
            {
                sb.AppendFormat("<c v='{0}' s='{1}' />", columnMapping.Name, columnMapping.TitleStyle);
            }
            sb.Append("</r>");
            #endregion

            #region Table Data
            foreach (DataRow dataRow in dataTable.Rows)
            {
                sb.Append("<r>");
                foreach (var columnMapping in columnMappings)
                {
                    sb.AppendFormat("<c v='{0}' />", dataRow[columnMapping.Value] == DBNull.Value ? string.Empty : dataRow[columnMapping.Value].ToString());
                }
                sb.Append("</r>");
            }
            #endregion

            sb.Append("</region>");
            #endregion

            sb.Append("</sheet>");
            sb.Append("</root>");

            return sb.ToString();
        }
    }
}
