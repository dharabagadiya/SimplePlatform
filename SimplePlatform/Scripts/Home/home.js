var home = {};
home.options = {
    FundRaisingURL: "/Home/GetFundRaisingTargets",
    BookingURL: "/Home/GetBookingTargets",
    GSBDataURL: "/Home/GetGSBTargets",
    ArrivalDataURL: "/Home/GetArrivalTargets"
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
            min: 0,
            title: { text: 'Target Amount (in $)' }
        },
        credits: { enabled: false },
        tooltip: { headerFormat: '<b>{series.name}</b><br>', pointFormat: '{point.x:%e, %b} : {point.y:.2f}$' },
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
            min: 0,
            title: { text: 'No. of Booking' }
        },
        credits: { enabled: false },
        tooltip: { headerFormat: '<b>{series.name}</b><br>', pointFormat: '{point.x:%e, %b} : {point.y:.2f}$' },
        plotOptions: { spline: { marker: { enabled: true } } },
        series: data
    });
};
home.LoadGSBChart = function (data) {
    var chartArea = $("#divGSBChart");
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
            min: 0,
            title: { text: 'Target Amount (in $)' }
        },
        credits: { enabled: false },
        tooltip: { headerFormat: '<b>{series.name}</b><br>', pointFormat: '{point.x:%e, %b} : {point.y:.2f}$' },
        plotOptions: { spline: { marker: { enabled: true } } },
        series: data
    });
};
home.LoadArrivalChart = function (data) {
    var chartArea = $("#divArrivalChart");
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
            min: 0,
            title: { text: 'No. of Arrival' }
        },
        credits: { enabled: false },
        tooltip: { headerFormat: '<b>{series.name}</b><br>', pointFormat: '{point.x:%e, %b} : {point.y:.2f}$' },
        plotOptions: { spline: { marker: { enabled: true } } },
        series: data
    });
};
home.GetFundRaisingData = function () {
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        url: home.options.FundRaisingURL,
        async: true,
        success: function (data) { home.LoadFundRaisingChart(data); }
    });
};
home.GetBookingData = function () {
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        url: home.options.BookingURL,
        async: true,
        success: function (data) { home.LoadBookingChart(data); }
    });
};
home.GetGSBTargetData = function () {
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        url: home.options.GSBDataURL,
        async: true,
        success: function (data) { home.LoadGSBChart(data); }
    });
};
home.GetArrivalTargetData = function () {
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        url: home.options.ArrivalDataURL,
        async: true,
        success: function (data) { home.LoadArrivalChart(data); }
    });
};
home.DoPageSetting = function () {
    this.GetFundRaisingData();
    this.GetBookingData();
    this.GetGSBTargetData();
    this.GetArrivalTargetData();
};
