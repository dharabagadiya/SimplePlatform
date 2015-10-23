using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePlatform.Controllers
{
    public class ReportController : BaseController
    {
        public ActionResult Index()
        {
            BundleConfig.AddScript("~/Scripts/Report", "report.js", ControllerName);
            StartupScript = "report.DoPageSetting();";
            return View();
        }

        public void ArrivalsReport(string startDate, string endDate)
        {
            var startDateTime = Convert.ToDateTime(startDate);
            var endDateTime = Convert.ToDateTime(endDate);
            var reportManager = new DataAccess.ReportManager();
            var officeManager = new DataAccess.OfficeMananer();
            var offices = officeManager.GetOfficeIDs(IsAdmin ? 0 : UserDetail.UserId);
            var arrivalsReportData = reportManager.GetArrivalAudiences(offices, startDateTime, endDateTime);
            var columnDetails = new List<Utilities.ExcelExport.ColumnDetail>();
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "FSM Name", Value = "FSMDetailName", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Name of Selected Person", Value = "PeopleName", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Continent Office", Value = "OfficeName", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Service Selected", Value = "ServiceName", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Date of arrival", Value = "VisitDate", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Status", Value = "ArrivalStatus", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "GBS", Value = "GSBAmount", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Funds", Value = "Amount", Type = "string", TitleStyle = "header1" });
            var excelDataXML = Utilities.ExcelExport.ExportExcel("Test -1", columnDetails, arrivalsReportData.Tables[0]);
            string templatePath = "~/ExportTemplate/Excel/Template.xlsx";
            string downloadFilename = "Test.xlsx";
            try
            {
                OfficeExports.Excel.UpdateWorkbook(excelDataXML, templatePath, downloadFilename, false);
            }
            catch (Exception ex)
            {

            }
        }
    }
}