var officeDetail = {};
var chartData = [{
    name: 'Tokyo',
    data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
}];
officeDetail.bookingProgress = new ProgressBar.Circle('#customer-exp', {
    color: '#0a97b9',
    strokeWidth: 2,
    fill: '#d0f1f9',
    duration: 4000,
    easing: 'bounce'
});
officeDetail.StockChart = function (chartData) {
    $('#StockChartContainer').highcharts({
        chart: { height: 290 },
        credits: {
            enabled: false
        },
        title: {
            text: 'Monthly Average',
            x: -20 //center
        },
        xAxis: {
            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
                'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
        },
        yAxis: {
            title: {
                text: ''
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        tooltip: {
            valueSuffix: '°C'
        },
        plotOptions: {
            line: {
                dataLabels: {
                    enabled: true
                },
                enableMouseTracking: false
            }
        },
        //legend: {
        //    layout: 'vertical',
        //    align: 'right',
        //    verticalAlign: 'middle',
        //    borderWidth: 0
        //},
        series: chartData
    });
}
officeDetail.GetTaskWidgetHTML = function (obj) {
    var sb = new StringBuilder();
    var colors = ["success", "default", "info", "warning", "danger"];
    var item = colors[Math.floor(Math.random() * colors.length)];
    sb.append("<li class=\"list-group-item clearfix comment-" + item + "\">");
    sb.append("<p class=\"text-ellipsis\"><span class=\"name strong\">" + obj.Name + ": </span> " + obj.Description + ".</p>");
    sb.append("<span class=\"date text-muted small pull-left\">End Date: " + obj.EndDate + "</span>");
    sb.append("</li>");
    return sb.toString();
}
officeDetail.TaskWidget = function (dataObj) {
    $(".taskWidget").empty();
    for (var i = 0; i < dataObj.length; i++) {
        widget = $(this.GetTaskWidgetHTML(dataObj[i]));
        $(".taskWidget").append(widget);
    };
}
officeDetail.GetTasksData = function () {
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        url: "/Offices/GetTasks",
        async: false,
        data: JSON.stringify({ "officeID": 1 }),
        success: function (data) {
            officeDetail.TaskWidget(data);
        }
    });
};
$(document).ready(function () {
    officeDetail.StockChart(chartData);
    officeDetail.bookingProgress.animate(0.5);
    officeDetail.GetTasksData();
});
