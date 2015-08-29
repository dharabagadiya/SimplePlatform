var officeDetail = {};
officeDetail.options = {
    colors: ["success", "default", "info", "warning", "danger"]
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
officeDetail.LoadBookingData = function () {
    var percentageCount = bookingTargetData.TotalTarget <= 0 ? 0 : Math.round(bookingTargetData.TotalTargetAchieved / bookingTargetData.TotalTarget);
    $("#divBookingChart").find(".percent").empty().html(percentageCount + "<span>%</span>");
    $("#divBookingChartContainer").find(".divTotalBookingTarget").empty().html(bookingTargetData.TotalTarget);
    $("#divBookingChartContainer").find(".divBookingTarget").empty().html(bookingTargetData.TotalTargetAchieved);
    officeDetail.bookingProgress.animate(percentageCount);
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
$(document).ready(function () {
    officeDetail.LoadFundRaisingChart(fundRaisingTargetData.ChartData);
    officeDetail.LoadBookingData();
    officeDetail.GetTaskList(tasks);
    officeDetail.GetAudienceList(arrivalAudiences);
});
