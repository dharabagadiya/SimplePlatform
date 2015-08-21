var home = {};
home.options = {
    FundRaisingURl: "/Home/GetFundRaisingTargets",
    BookingURl: "/Home/GetBookingTargets"
};
home.LoadFundRaisingChart = function (data) {
    var chartArea = $("#divFundRaisingChart");
    chartArea.highcharts({
        chart: {},
        title: { text: "" },
        subtitle: { text: "" },

        xAxis: {
            type: 'datetime',
            dateTimeLabelFormats: { month: '%b %e, %Y' },
            title: { text: "Due Date" }
        },
        yAxis: {
            title: { text: 'Target Amount (in $)' }
        },
        credits: { enabled: false },
        tooltip: { headerFormat: '<b>{series.name}</b><br>', pointFormat: '{point.x:%e. %b}: {point.y:.2f} m' },
        plotOptions: { spline: { marker: { enabled: true } } },
        series: data
    });
};
home.LoadBookingChart = function (data) {
    var chartArea = $("#divBookingChart");
    chartArea.highcharts({
        chart: {},
        title: { text: "" },
        subtitle: { text: "" },

        xAxis: {
            type: 'datetime',
            dateTimeLabelFormats: { month: '%b %e, %Y' },
            title: { text: "Due Date" }
        },
        yAxis: {
            title: { text: 'No. of Booking' }
        },
        credits: { enabled: false },
        tooltip: { headerFormat: '<b>{series.name}</b><br>', pointFormat: '{point.x:%e. %b}: {point.y:.2f} m' },
        plotOptions: { spline: { marker: { enabled: true } } },
        series: data
    });
};
home.GetFundRaisingData = function () {
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        url: home.options.FundRaisingURl,
        async: true,
        success: function (data) { home.LoadFundRaisingChart(data); }
    });
};
home.GetBookingData = function () {
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        url: home.options.BookingURl,
        async: true,
        success: function (data) { home.LoadBookingChart(data); }
    });
};
home.DoPageSetting = function () {
    this.GetFundRaisingData();
    this.GetBookingData();
};