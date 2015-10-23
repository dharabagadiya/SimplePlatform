var report = {};
report.options = {
    startDate: null,
    endDate: null,
    arrivalReportURL: "/Report/ArrivalsReport/"

}

report.ArrivalReportDownload = function () {
    var formObj = $("#frmDownloadReport");
    formObj.find("#startDate").val(report.options.startDate.toDateString());
    formObj.find("#endDate").val(report.options.endDate.toDateString());
    formObj.attr("action", report.options.arrivalReportURL)
    formObj.submit();
};

report.UpdateGlobalTimePeriodSelection = function (start, end) {
    report.options.startDate = start.toDate();
    report.options.endDate = end.toDate();
    $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
};
report.LoadGlobalTimeFilter = function () {
    $('#reportrange').daterangepicker({
        "startDate": moment().startOf('week').isoWeekday(4),
        "endDate": moment().endOf('week').isoWeekday(4),
        ranges: {
            'Last 7 Days': [moment().startOf('week').isoWeekday(4), moment().endOf('week').isoWeekday(4)],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, report.UpdateGlobalTimePeriodSelection).off("apply.daterangepicker").on('apply.daterangepicker', function (ev, picker) {
        //audiences.LoadAudienceList();
    });
    report.UpdateGlobalTimePeriodSelection(moment().startOf('week').isoWeekday(4), moment().endOf('week').isoWeekday(4));
    //report.LoadAudienceList();
};
report.DoPageSetting = function () {
    report.LoadGlobalTimeFilter();
    //audiences.LoadQuickBooking();
};