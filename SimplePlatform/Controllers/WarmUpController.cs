using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimplePlatform.Controllers
{
    public class WarmUpController : Controller
    {
        public PartialViewResult TestMail()
        {
            ViewData["Controller_Name"] = "WarmUp";
            new Utilities.Email.Invoke((new Utilities.Email()).SendMail).BeginInvoke("mehul.patel20010@gmail.com", "mehul.chandroliya@gmail.com", "Selection Slip / ASR", "Test", null, null);
            return PartialView();
        }

        public PartialViewResult SendRecapMail()
        {
            var reportManager = new DataAccess.ReportManager();
            var fsmMaanger = new DataAccess.FSMDetailManager();
            var ids = reportManager.GetSelectedFSMIDs();
            foreach (var id in ids)
            {
                using (var sw = new StringWriter())
                {
                    var fsmAudienceDetail = reportManager.GetFSMWeeklyCumulativeStats(id);
                    var fsmDetail = fsmMaanger.FSMDetail(id);
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
                    columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Arrived", Value = "ArrivalStatus", Type = "string", TitleStyle = "header1" });
                    columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "GBS", Value = "GSBAmount", Type = "currencyamount", TitleStyle = "header1" });
                    columnDetails.Add(new Utilities.ExcelExport.ColumnDetail { Name = "Funds", Value = "Amount", Type = "currencyamount", TitleStyle = "header1" });

                    regions["Booked"] = new Utilities.ExcelExport.RegionDetail { Title = "Booked", TitleStyle = "header1", ColumnDetail = columnDetails };
                    regions["InProcess"] = new Utilities.ExcelExport.RegionDetail { Title = "In Process", TitleStyle = "header1", ColumnDetail = columnDetails };
                    regions["Reachies"] = new Utilities.ExcelExport.RegionDetail { Title = "Reachies", TitleStyle = "header1", ColumnDetail = columnDetails };
                    regions["Arrivals"] = new Utilities.ExcelExport.RegionDetail { Title = "Arrivals", TitleStyle = "header1", ColumnDetail = columnDetails };
                    #endregion

                    #region View Data
                    ViewData["Table_Recap_Detail"] = Utilities.ExcelExport.ExportToTableHTML(regions, fsmAudienceDetail);
                    ViewData["FSM_Name"] = fsmDetail.Name;
                    #endregion

                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, "WeeklyRecap");
                    ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                    try
                    {
                        viewResult.View.Render(viewContext, sw);
                        new Utilities.Email.Invoke((new Utilities.Email()).SendMail).BeginInvoke(fsmDetail.EmailID, string.Empty, "Weekly Recap", sw.GetStringBuilder().ToString(), null, null);
                    }
                    catch (Exception ex)
                    { }
                }
            }
            return PartialView();
        }
    }
}