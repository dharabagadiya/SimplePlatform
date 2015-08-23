var officeDetail = {};
officeDetail.bookingProgress = new ProgressBar.Circle('#customer-exp', { color: '#0a97b9', strokeWidth: 2, fill: '#d0f1f9', duration: 4000, easing: 'bounce' });
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

$(document).ready(function () {
    officeDetail.LoadFundRaisingChart(fundRaisingTargetData.ChartData);
    officeDetail.bookingProgress.animate(0.5);
});
