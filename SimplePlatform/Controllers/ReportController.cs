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
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Week Ending", Value = "EndDate", Type = "dateformat", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "FSM Name", Value = "FSMDetailName", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Name of Selected Person", Value = "PeopleName", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Continent Office", Value = "OfficeName", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Service Selected", Value = "ServiceName", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Date of arrival", Value = "VisitDate", Type = "dateformat", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Status", Value = "ArrivalStatus", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "GBS", Value = "GSBAmount", Type = "currencyamount", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Funds", Value = "Amount", Type = "currencyamount", TitleStyle = "header1" });
            var excelDataXML = Utilities.ExcelExport.ExportExcel("People of Arrivals From", columnDetails, arrivalsReportData.Tables[0]);
            string templatePath = "~/ExportTemplate/Excel/Template.xlsx";
            string downloadFilename = string.Format("People of Arrivals From {0} to {1}", startDateTime.ToString("MMM-dd-yyyy"), endDateTime.ToString("MMM-dd-yyyy"));
            try
            {
                OfficeExports.Excel.UpdateWorkbook(excelDataXML, templatePath, downloadFilename, false);
            }
            catch (Exception ex)
            {

            }
        }

        public void SelectionByFSMSelection(string startDate, string endDate)
        {
            var startDateTime = Convert.ToDateTime(startDate);
            var endDateTime = Convert.ToDateTime(endDate);
            var reportManager = new DataAccess.ReportManager();
            var officeManager = new DataAccess.OfficeMananer();
            var offices = officeManager.GetOfficeIDs(IsAdmin ? 0 : UserDetail.UserId);
            var arrivalsReportData = reportManager.GetSelectionByFSMSelection(offices, startDateTime, endDateTime);
            var columnDetails = new List<Utilities.ExcelExport.ColumnDetail>();
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Week Ending", Value = "EndDate", Type = "dateformat", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "FSM Name", Value = "FSMDetailName", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Name of Selected Person", Value = "PeopleName", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Continent Office", Value = "OfficeName", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Service Selected", Value = "ServiceName", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Date of arrival", Value = "VisitDate", Type = "dateformat", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Status", Value = "ArrivalStatus", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "GBS", Value = "GSBAmount", Type = "currencyamount", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Funds", Value = "Amount", Type = "currencyamount", TitleStyle = "header1" });
            var excelDataXML = Utilities.ExcelExport.ExportExcel("Selection BY FSM From", columnDetails, arrivalsReportData.Tables[0]);
            string templatePath = "~/ExportTemplate/Excel/Template.xlsx";
            string downloadFilename = string.Format("Selection BY FSM From {0} to {1}", startDateTime.ToString("MMM-dd-yyyy"), endDateTime.ToString("MMM-dd-yyyy"));
            try
            {
                OfficeExports.Excel.UpdateWorkbook(excelDataXML, templatePath, downloadFilename, false);
            }
            catch (Exception ex)
            {

            }
        }

        public void SelectionSlipGeneral(string startDate, string endDate)
        {
            var startDateTime = Convert.ToDateTime(startDate);
            var endDateTime = Convert.ToDateTime(endDate);
            var reportManager = new DataAccess.ReportManager();
            var officeManager = new DataAccess.OfficeMananer();
            var offices = officeManager.GetOfficeIDs(IsAdmin ? 0 : UserDetail.UserId);
            var arrivalsReportData = reportManager.GetSelectionByFSMSelection(offices, startDateTime, endDateTime);
            var columnDetails = new List<Utilities.ExcelExport.ColumnDetail>();
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Week Ending", Value = "EndDate", Type = "dateformat", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "FSM Name", Value = "FSMDetailName", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Name of Selected Person", Value = "PeopleName", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Continent Office", Value = "OfficeName", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Service Selected", Value = "ServiceName", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Date of arrival", Value = "VisitDate", Type = "dateformat", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Status", Value = "ArrivalStatus", Type = "string", TitleStyle = "header1" });
            //columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "GBS", Value = "GSBAmount", Type = "currencyamount", TitleStyle = "header1" });
            //columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Funds", Value = "Amount", Type = "currencyamount", TitleStyle = "header1" });
            var excelDataXML = Utilities.ExcelExport.ExportExcel("Selection Slip General", columnDetails, arrivalsReportData.Tables[0]);
            string templatePath = "~/ExportTemplate/Excel/Template.xlsx";
            string downloadFilename = string.Format("Selection Slip General From {0} to {1}", startDateTime.ToString("MMM-dd-yyyy"), endDateTime.ToString("MMM-dd-yyyy"));
            try
            {
                OfficeExports.Excel.UpdateWorkbook(excelDataXML, templatePath, downloadFilename, false);
            }
            catch (Exception ex)
            {

            }
        }

        public void WeeklyCumulativeStateByFSM(string startDate, string endDate)
        {
            var startDateTime = Convert.ToDateTime(startDate);
            var endDateTime = Convert.ToDateTime(endDate);
            var reportManager = new DataAccess.ReportManager();
            var officeManager = new DataAccess.OfficeMananer();
            var offices = officeManager.GetOfficeIDs(IsAdmin ? 0 : UserDetail.UserId);
            var arrivalsReportData = reportManager.GetWeeklyCumulativeStateByFSM(offices, startDateTime, endDateTime);
            var regions = new Dictionary<string, Utilities.ExcelExport.RegionDetail>();

            #region State Report
            var stateColumnDetails = new List<Utilities.ExcelExport.ColumnDetail>();
            stateColumnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Week Ending", Value = "EndDate", Type = "dateformat", TitleStyle = "header1" });
            stateColumnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "FSM Name", Value = "FSMDetailName", Type = "string", TitleStyle = "header1" });
            stateColumnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Funds Rised", Value = "FundRaised", Type = "currencyamount", TitleStyle = "header1" });
            stateColumnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "GBS", Value = "GSBAmount", Type = "currencyamount", TitleStyle = "header1" });
            stateColumnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Reaches", Value = "ReachesCount", Type = "string", TitleStyle = "header1" });
            stateColumnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "In Progress", Value = "InProcessCount", Type = "string", TitleStyle = "header1" });
            stateColumnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Booked", Value = "BookedCount", Type = "string", TitleStyle = "header1" });
            stateColumnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Arrivals", Value = "ArrivalCount", Type = "string", TitleStyle = "header1" });
            regions["CumulativeState"] = new Utilities.ExcelExport.RegionDetail { Title = "Week Cumulative State", TitleStyle = "header1", ColumnDetail = stateColumnDetails };
            #endregion

            #region Individual Report
            var columnDetails = new List<Utilities.ExcelExport.ColumnDetail>();
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Week Ending", Value = "EndDate", Type = "dateformat", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "FSM Name", Value = "FSMDetailName", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Name of Selected Person", Value = "PeopleName", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Continent Office", Value = "OfficeName", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Service Selected", Value = "ServiceName", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Date of arrival", Value = "VisitDate", Type = "dateformat", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Status", Value = "ArrivalStatus", Type = "string", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "GBS", Value = "GSBAmount", Type = "currencyamount", TitleStyle = "header1" });
            columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Funds", Value = "Amount", Type = "currencyamount", TitleStyle = "header1" });

            regions["Arrivals"] = new Utilities.ExcelExport.RegionDetail { Title = "Arrivals", TitleStyle = "header1", ColumnDetail = columnDetails };
            regions["Booked"] = new Utilities.ExcelExport.RegionDetail { Title = "Booked", TitleStyle = "header1", ColumnDetail = columnDetails };
            regions["InProcess"] = new Utilities.ExcelExport.RegionDetail { Title = "In Process", TitleStyle = "header1", ColumnDetail = columnDetails };
            regions["Reachies"] = new Utilities.ExcelExport.RegionDetail { Title = "Reachies", TitleStyle = "header1", ColumnDetail = columnDetails };
            #endregion


            var excelDataXML = Utilities.ExcelExport.ExportExcel("Weekly Cumulative State By FSM", regions, arrivalsReportData);
            string templatePath = "~/ExportTemplate/Excel/Template.xlsx";
            string downloadFilename = string.Format("Weekly Cumulative State By FSM From {0} to {1}", startDateTime.ToString("MMM-dd-yyyy"), endDateTime.ToString("MMM-dd-yyyy"));
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