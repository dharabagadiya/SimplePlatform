var report = {};
report.options = {
    startDate: null,
    endDate: null,
    arrivalReportURL: "/Report/ArrivalsReport/",
    selectionByFSMSelectionReportURL: "/Report/SelectionByFSMSelection",
    selectionSlipGeneralReportURL: "/Report/SelectionSlipGeneral",
    weeklyCumulativeStateByFSMReportURL: "/Report/WeeklyCumulativeStateByFSM",
    addWeek: ((new Date().getDay()) <= 4 ? 0 : 1),
    subtractWeek: ((new Date().getDay()) <= 4 ? 1 : 0)
}

report.ArrivalReportDownload = function () {
    var formObj = $("#frmDownloadReport");
    formObj.find("#startDate").val(report.options.startDate.toDateString());
    formObj.find("#endDate").val(report.options.endDate.toDateString());
    formObj.attr("action", report.options.arrivalReportURL);
    formObj.submit();
};
report.SelectionByFSMReportDownload = function () {
    var formObj = $("#frmDownloadReport");
    formObj.find("#startDate").val(report.options.startDate.toDateString());
    formObj.find("#endDate").val(report.options.endDate.toDateString());
    formObj.attr("action", report.options.selectionByFSMSelectionReportURL);
    formObj.submit();
};
report.SelectionSlipGeneralReportDownload = function () {
    var formObj = $("#frmDownloadReport");
    formObj.find("#startDate").val(report.options.startDate.toDateString());
    formObj.find("#endDate").val(report.options.endDate.toDateString());
    formObj.attr("action", report.options.selectionSlipGeneralReportURL);
    formObj.submit();
};
report.WeeklyCumulativeStateByFSMReportDownload = function () {
    var formObj = $("#frmDownloadReport");
    formObj.find("#startDate").val(report.options.startDate.toDateString());
    formObj.find("#endDate").val(report.options.endDate.toDateString());
    formObj.attr("action", report.options.weeklyCumulativeStateByFSMReportURL);
    formObj.submit();
};

report.UpdateGlobalTimePeriodSelection = function (start, end) {
    report.options.startDate = start.toDate();
    report.options.endDate = end.toDate();
    $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
};
report.LoadGlobalTimeFilter = function () {
    $('#reportrange').daterangepicker({
        "startDate": moment().startOf('week').isoWeekday(4).add(report.options.addWeek, "week"),
        "endDate": moment().endOf('week').isoWeekday(4).add(report.options.addWeek, "week"),
        ranges: {
            'Current Week': [moment().startOf('week').isoWeekday(4).add(report.options.addWeek, "week"), moment().endOf('week').isoWeekday(4).add(report.options.addWeek, "week")],
            'Last 7 Days': [moment().startOf('week').isoWeekday(4).subtract(report.options.subtractWeek, "week"), moment().endOf('week').isoWeekday(4).subtract(report.options.subtractWeek, "week")],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, report.UpdateGlobalTimePeriodSelection).off("apply.daterangepicker").on('apply.daterangepicker', function (ev, picker) {
        //audiences.LoadAudienceList();
    });
    report.UpdateGlobalTimePeriodSelection(moment().startOf('week').isoWeekday(4).add(report.options.addWeek, "week"), moment().endOf('week').isoWeekday(4).add(report.options.addWeek, "week"));
    //report.LoadAudienceList();
};
report.DoPageSetting = function () {
    report.LoadGlobalTimeFilter();
    //audiences.LoadQuickBooking();
};