var officeDetail = {};
officeDetail.options = {
    officeID: 0,
    GetDetailURL: "/Offices/Detail",
    colors: ["success", "default", "info", "warning", "danger"],
    startDate: null,
    endDate: null
};
officeDetail.bookingProgress = new ProgressBar.Circle('#divBookingChart', { color: '#0a97b9', strokeWidth: 2, fill: '#d0f1f9', duration: 4000, easing: 'bounce' });
officeDetail.NoRecordFound = function (message) {
    var sb = new StringBuilder();
    sb.append("<li class=\"list-group-item clearfix\">");
    sb.append("<span class=\"date text-muted small text-center\">" + message + "</span>");
    sb.append("</li>");
    return sb.toString();
}
officeDetail.LoadFundRaisingChart = function (data) {
    var chartArea = $("#divStockChartContainer");
    chartArea.highcharts({
        chart: {},
        title: { text: "" },
        subtitle: { text: "" },
        xAxis: {
            type: 'datetime',
            dateTimeLabelFormats: { month: '%b %e, %Y' },
            title: { text: "" }
        },
        yAxis: {
            min: 0,
            title: { text: 'Target Amount (in $)' }
        },
        credits: { enabled: false },
        tooltip: { headerFormat: '<b>{series.name}</b><br>', pointFormat: '{point.x:%e, %b} : {point.y:.2f}$' },
        plotOptions: { spline: { marker: { enabled: true } } },
        series: data
    });
};
officeDetail.LoadBookingData = function (bookingTargetData) {
    var percentageCount = bookingTargetData.Total <= 0 ? 0 : (bookingTargetData.ActTotal / bookingTargetData.Total);
    $("#divBookingChart").find(".percent").empty().html((percentageCount.toFixed(2)) + "<span>%</span>");
    $("#divBookingChartContainer").find(".divTotalBookingTarget").empty().html(bookingTargetData.Total);
    $("#divBookingChartContainer").find(".divBookingTarget").empty().html(bookingTargetData.ActTotal);
    officeDetail.bookingProgress.animate(percentageCount.toFixed(2));
};
officeDetail.GetTaskWidgetHTML = function (obj) {
    var sb = new StringBuilder();
    var item = officeDetail.options.colors[Math.floor(Math.random() * officeDetail.options.colors.length)];
    sb.append("<li class=\"list-group-item clearfix comment-" + item + "\">");
    sb.append("<p class=\"text-ellipsis\"><span class=\"name strong\">" + obj.Name + "</span></p>");
    sb.append("<span class=\"date text-muted small pull-left\">Due Date - " + obj.EndDate + "</span>");
    sb.append("</li>");
    return sb.toString();
}
officeDetail.GetTaskList = function (dataObj) {
    var tasksWidget = $(".taskWidget").empty();
    if (dataObj.length <= 0) { tasksWidget.append(this.NoRecordFound("No Task available for this week.")); return; }
    for (var i = 0; i < dataObj.length; i++) { widget = $(this.GetTaskWidgetHTML(dataObj[i])); tasksWidget.append(widget); };
};
officeDetail.GetAudienceWidgetHTML = function (obj) {
    var sb = new StringBuilder();
    var item = officeDetail.options.colors[Math.floor(Math.random() * officeDetail.options.colors.length)];
    sb.append("<li class=\"list-group-item clearfix comment-" + item + "\">");
    sb.append("<p class=\"text-ellipsis\"><span class=\"name strong\">" + obj.Name + "</span></p>");
    sb.append("<span class=\"date small pull-left\">Attend: " + obj.ConventionName + "&nbsp;</span>");
    sb.append("<span class=\"date text-muted small pull-left\">By " + obj.ArrivalDate + "</span>");
    sb.append("</li>");
    return sb.toString();
}
officeDetail.GetAudienceList = function (dataObj) {
    var audienceWidget = $(".audienceWidget").empty();
    if (dataObj.length <= 0) { audienceWidget.append(this.NoRecordFound("No Convention this week")); return; }
    for (var i = 0; i < dataObj.length; i++) { widget = $(this.GetAudienceWidgetHTML(dataObj[i])); audienceWidget.append(widget); };
};
officeDetail.UpdatePageDataByFilter = function () {
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        url: officeDetail.options.GetDetailURL,
        async: true,
        data: JSON.stringify({ id: officeDetail.options.officeID, startDate: officeDetail.options.startDate, endDate: officeDetail.options.endDate }),
        success: function (dataObj) {
            officeDetail.LoadFundRaisingChart(dataObj.fundRaisingTargetData.ChartData);
            officeDetail.LoadBookingData(dataObj.bookingTargetData);
            officeDetail.GetTaskList(dataObj.tasks);
            officeDetail.GetAudienceList(dataObj.arrivalAudiences);
        }
    });
};
officeDetail.UpdateGlobalTimePeriodSelection = function (start, end) {
    officeDetail.options.startDate = start.toDate();
    officeDetail.options.endDate = end.toDate();
    $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
};
officeDetail.LoadGlobalTimeFilter = function () {
    $('#reportrange').daterangepicker({
        "startDate": moment().startOf('week').subtract(2, 'days'),
        "endDate": moment().startOf('week').add('days', 4),
        ranges: {
            'Last 7 Days': [moment().startOf('week').subtract(2, 'days'), moment().startOf('week').add('days', 4)],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, officeDetail.UpdateGlobalTimePeriodSelection).off("apply.daterangepicker").on('apply.daterangepicker', function (ev, picker) { officeDetail.UpdatePageDataByFilter(); });
    officeDetail.UpdateGlobalTimePeriodSelection(moment().startOf('week').subtract(2, 'days'), moment().startOf('week').add('days', 4));
    officeDetail.UpdatePageDataByFilter();
};

$(document).ready(function () {
    officeDetail.LoadGlobalTimeFilter();

});
