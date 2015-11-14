
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

        public class RegionDetail
        {
            public string Title { get; set; }
            public List<ColumnDetail> ColumnDetail { get; set; }
            public string TitleStyle { get; set; }
        }

        public static string GetDataTypeFormat(string type)
        {
            switch (type)
            {
                case "dateformat": return "s=\"dateformat\"";
                case "currencyamount": return "s=\"currencyamount\"";
                default: return string.Empty;
            }
        }

        public static string ExportExcel(string sheetName, List<ColumnDetail> columnMappings, DataTable dataTable)
        {
            var sb = new StringBuilder("<root>");
            sb.AppendFormat("<sheet name = \"Sheet1\" cellstylemode = \"name\" renameto=\"{0}\">", sheetName);

            #region Table Detail
            sb.Append("<region startat=\"a1\">");

            #region Table Title
            sb.Append("<r>");
            foreach (var columnMapping in columnMappings)
            {
                sb.AppendFormat("<c v=\"{0}\" s=\"{1}\" />", columnMapping.Name, columnMapping.TitleStyle);
            }
            sb.Append("</r>");
            #endregion

            #region Table Data
            foreach (DataRow dataRow in dataTable.Rows)
            {
                sb.Append("<r>");
                foreach (var columnMapping in columnMappings)
                {
                    sb.AppendFormat("<c v=\"{0}\" {1}/>", dataRow[columnMapping.Value] == DBNull.Value ? string.Empty : dataRow[columnMapping.Value].ToString(), GetDataTypeFormat(columnMapping.Type));
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

        public static string ExportExcel(string sheetName, Dictionary<string, RegionDetail> dicColumnMappings, DataSet dataSet)
        {
            var sb = new StringBuilder("<root>"); var startRow = 1; var tableIndex = 0;
            sb.AppendFormat("<sheet name = \"Sheet1\" cellstylemode = \"name\" renameto=\"{0}\">", sheetName);

            foreach (var key in dicColumnMappings.Keys)
            {
                if ((tableIndex + 1) > dataSet.Tables.Count) { continue; }

                var regionDetail = dicColumnMappings[key];
                var columnMappings = regionDetail.ColumnDetail;
                var dataTable = dataSet.Tables[tableIndex];

                #region Table Detail
                sb.AppendFormat("<region startat=\"a{0}\">", startRow);

                #region Table Title
                sb.Append("<r>");
                sb.AppendFormat("<c v=\"{0}\" s=\"{1}\" />", regionDetail.Title, regionDetail.TitleStyle);
                sb.Append("</r>");

                sb.Append("<r>");
                foreach (var columnMapping in columnMappings)
                {
                    sb.AppendFormat("<c v=\"{0}\" s=\"{1}\" />", columnMapping.Name, columnMapping.TitleStyle);
                }
                sb.Append("</r>");
                #endregion

                #region Table Data
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    sb.Append("<r>");
                    foreach (var columnMapping in columnMappings)
                    {
                        sb.AppendFormat("<c v=\"{0}\" {1}/>", dataRow[columnMapping.Value] == DBNull.Value ? string.Empty : dataRow[columnMapping.Value].ToString(), GetDataTypeFormat(columnMapping.Type));
                    }
                    sb.Append("</r>");
                }
                #endregion

                sb.Append("</region>");
                #endregion

                startRow += (dataTable.Rows.Count + 5);
                tableIndex += 1;
            }


            sb.Append("</sheet>");
            sb.Append("</root>");

            return sb.ToString();
        }

        public static string ExportToTableHTML(Dictionary<string, RegionDetail> dicColumnMappings, DataSet dataSet)
        {
            var sb = new StringBuilder();
            var tableIndex = 0;
            foreach (var key in dicColumnMappings.Keys)
            {
                var dataTable = dataSet.Tables[tableIndex];
                var regionDetail = dicColumnMappings[key];
                var columnMappings = regionDetail.ColumnDetail;

                sb.AppendFormat("<h3>{0}</h3>", regionDetail.Title);
                sb.Append("<table border=\"1\" style=\"border-collapse: collapse;\">");

                // Table Column
                sb.Append("<tr>");
                foreach (var columnMapping in columnMappings)
                {
                    sb.AppendFormat("<td>{0}</td>", columnMapping.Name);
                }
                sb.Append("</tr>");

                //add rows
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    sb.Append("<tr>");
                    foreach (var columnMapping in columnMappings)
                    {
                        var value = string.Empty;
                        var type = columnMapping.Type;
                        switch (type)
                        {
                            case "dateformat":
                                value = dataRow[columnMapping.Value] == DBNull.Value ? string.Empty : dataRow.Field<DateTime>(columnMapping.Value).ToString("MMM dd, yyyyy");
                                break;
                            case "currencyamount":
                                value = ("$" + (dataRow[columnMapping.Value] == DBNull.Value ? "0" : dataRow[columnMapping.Value].ToString()));
                                break;
                            default:
                                value = dataRow[columnMapping.Value] == DBNull.Value ? string.Empty : dataRow[columnMapping.Value].ToString();
                                break;
                        }
                        sb.AppendFormat("<td>{0}</td>", value);
                    }
                    sb.Append("</tr>");
                }
                sb.Append("</table>");
                sb.Append("<br/></br/><br/>");
                tableIndex += 1;
            }
            return sb.ToString();
        }
    }
}
