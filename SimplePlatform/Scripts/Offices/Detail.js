var officeDetail = {};
officeDetail.options = {
    colors: ["success", "default", "info", "warning", "danger"]
};
officeDetail.bookingProgress = new ProgressBar.Circle('#divBookingChart', { color: '#0a97b9', strokeWidth: 2, fill: '#d0f1f9', duration: 4000, easing: 'bounce' });
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
    var percentageCount = Math.round(bookingTargetData.TotalTargetAchieved / bookingTargetData.TotalTarget);
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
    sb.append("<span class=\"date text-muted small pull-left\">End Date: " + obj.EndDate + "</span>");
    sb.append("</li>");
    return sb.toString();
}
officeDetail.GetTaskList = function (dataObj) {
    var tasksWidget = $(".taskWidget").empty();
    for (var i = 0; i < dataObj.length; i++) { widget = $(this.GetTaskWidgetHTML(dataObj[i])); tasksWidget.append(widget); };
};
$(document).ready(function () {
    officeDetail.LoadFundRaisingChart(fundRaisingTargetData.ChartData);
    officeDetail.LoadBookingData();
    officeDetail.GetTaskList(tasks);
});
