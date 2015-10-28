var officeDetail = {};
officeDetail.options = {
    officeID: 0,
    GetDetailURL: "/Offices/Detail",
    UpdateTaskStatusURL: "/Tasks/Status",
    UpdateAttendStatusURL: "/Audiences/AttendStatus",
    UpdateAudienceAttendStatusURL: "/Audiences/UpdateAudienceStatus",
    colors: ["success", "default", "info", "warning", "danger"],
    startDate: null,
    endDate: null,
    addWeek: ((new Date().getDay()) <= 4 ? 0 : 1),
    subtractWeek: ((new Date().getDay()) <= 4 ? 1 : 0)
};
officeDetail.bookingProgress = new ProgressBar.Circle('#divBookingChart', { color: '#0a97b9', strokeWidth: 2, fill: '#d0f1f9', duration: 4000, easing: 'bounce' });
officeDetail.AttachScrollBar = function (obj) {
    obj.niceScroll({
        cursorcolor: "#999",
        cursoropacitymin: 0,
        cursoropacitymax: 0.3,
        cursorwidth: 5,
        cursorborder: "0px",
        cursorborderradius: "0px",
        cursorminheight: 50,
        zindex: 1,
        mousescrollstep: 20
    });
}
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
officeDetail.UpdateTaskStatus = function (obj) {
    obj.find(".lnkMarkTaskButton").off("click.lnkMarkTaskButton").on("click.lnkMarkTaskButton", function () {
        var currentObj = $(this);
        var taskID = parseInt(currentObj.attr("data-task-ID"));
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            async: true,
            data: JSON.stringify({ "id": taskID }),
            url: officeDetail.options.UpdateTaskStatusURL,
            success: function (data) {
                if (currentObj.hasClass("ion-android-checkbox-outline")) { currentObj.removeClass("ion-android-checkbox-outline").addClass("ion-android-checkbox-outline-blank"); }
                else { currentObj.addClass("ion-android-checkbox-outline").removeClass("ion-android-checkbox-outline-blank"); }
            }
        });
    });
};
officeDetail.GetTaskWidgetHTML = function (obj) {
    var sb = new StringBuilder();
    var item = officeDetail.options.colors[Math.floor(Math.random() * officeDetail.options.colors.length)];
    sb.append("<li class=\"list-group-item clearfix comment-" + item + "\">");
    sb.append("<p class=\"text-ellipsis\"><a href=\"#\"><i data-task-ID=\"" + obj.ID + "\" class=\"lnkMarkTaskButton name icon " + (obj.IsCompleted ? "ion-android-checkbox-outline" : "ion-android-checkbox-outline-blank") + "\"></i></a>&nbsp;<span class=\"name strong\">" + obj.Name + "</span></p>");
    sb.append("<span class=\"date text-muted small pull-left\">Due Date - " + obj.EndDate + "</span>");
    sb.append("<span class=\"date text-muted small pull-right\">Assign To " + obj.AssignTo + "</span>");
    sb.append("</li>");
    return sb.toString();
}
officeDetail.GetTaskList = function (dataObj) {
    var tasksWidget = $(".taskWidget").empty();
    if (dataObj.length <= 0) { tasksWidget.append(this.NoRecordFound("No Task available for this week.")); return; }
    for (var i = 0; i < dataObj.length; i++) { var widget = $(this.GetTaskWidgetHTML(dataObj[i])); tasksWidget.append(widget); };
    officeDetail.UpdateTaskStatus(tasksWidget);
};
officeDetail.UpdateAudienceStatus = function (obj) {
    obj.find(".lnkMarkAudienceButton").off("click.lnkMarkAudienceButton").on("click.lnkMarkAudienceButton", function () {
        var currentObj = $(this);
        var audienceID = parseInt(currentObj.attr("data-audience-id"));
        var status = currentObj.hasClass("ion-android-checkbox-outline");
        $("#divCommonModalPlaceHolder").empty();
        ShowDialogBox($("#divCommonModalPlaceHolder"), officeDetail.options.UpdateAudienceAttendStatusURL, null, $.proxy(function (event, dialogContentPlaceHolder) {
            dialogContentPlaceHolder.find(".txtVisitDate").datepicker({ autoclose: true, todayHighlight: true });
            if (status) {
                dialogContentPlaceHolder.find("#divArrivalDate").hide();
                dialogContentPlaceHolder.find(".modal-body").hide();
                dialogContentPlaceHolder.find("#btnUpdateStatus").html("Un-Mark As Arrived");
            } else {
                dialogContentPlaceHolder.find("#divArrivalDate").show();
                dialogContentPlaceHolder.find(".modal-body").show();
                dialogContentPlaceHolder.find("#btnUpdateStatus").html("Mark As Arrived");
            }
            dialogContentPlaceHolder.find("form").bootstrapValidator({
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                }
            }).off('success.form.bv').on('success.form.bv', function (e) {
                e.preventDefault();
                var formObj = $(e.target);
                var arrivalDate = formObj.find(".txtArrivalDate").val();
                $.ajax({
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    async: false,
                    data: JSON.stringify({ "id": audienceID, arrivalDateTime: arrivalDate }),
                    url: officeDetail.options.UpdateAttendStatusURL,
                    success: function (data) {
                        dialogContentPlaceHolder.modal("hide");
                        if (currentObj.hasClass("ion-android-checkbox-outline")) { currentObj.removeClass("ion-android-checkbox-outline").addClass("ion-android-checkbox-outline-blank"); }
                        else { currentObj.addClass("ion-android-checkbox-outline").removeClass("ion-android-checkbox-outline-blank"); }
                    }
                });
            });
        }, this));
    });
};
officeDetail.GetAudienceWidgetHTML = function (obj) {
    var sb = new StringBuilder();
    var item = officeDetail.options.colors[Math.floor(Math.random() * officeDetail.options.colors.length)];
    sb.append("<li class=\"list-group-item clearfix comment-" + item + "\">");
    sb.append("<p class=\"text-ellipsis\"><a href=\"#\"><i data-audience-id=\"" + obj.ID + "\" class=\"lnkMarkAudienceButton name icon " + (obj.IsAttended ? "ion-android-checkbox-outline" : "ion-android-checkbox-outline-blank") + "\"></i></a>&nbsp;<span class=\"name strong\">" + obj.Name + "</span></p>");
    sb.append("<span class=\"date small pull-left\">Attend " + obj.ConventionName + "&nbsp;</span>");
    sb.append("<span class=\"date text-muted small pull-left\">By " + obj.ArrivalDate + "</span>");
    sb.append("</li>");
    return sb.toString();
}
officeDetail.GetAudienceList = function (dataObj) {
    var audienceWidget = $(".audienceWidget").empty();
    if (dataObj.length <= 0) { audienceWidget.append(this.NoRecordFound("No Convention this week")); return; }
    for (var i = 0; i < dataObj.length; i++) { widget = $(this.GetAudienceWidgetHTML(dataObj[i])); audienceWidget.append(widget); };
    officeDetail.UpdateAudienceStatus(audienceWidget);
};
officeDetail.GetEventWidgetHTML = function (obj) {
    var sb = new StringBuilder();
    var item = officeDetail.options.colors[Math.floor(Math.random() * officeDetail.options.colors.length)];
    sb.append("<li class=\"list-group-item clearfix comment-" + item + "\">");
    sb.append("<p class=\"text-ellipsis\"><span class=\"name strong\">" + obj.Name + "</span></p>");
    sb.append("<span class=\"date small pull-left\">Attend From " + obj.StartDate + " To " + obj.EndDate + "</span>");
    sb.append("<span class=\"date text-muted small pull-right\">Total People Arrival " + obj.TotalPeopleAttended + "</span>");
    sb.append("</li>");
    return sb.toString();
}
officeDetail.GetEventList = function (dataObj) {
    var eventWidget = $(".eventWidget").empty();
    if (dataObj.length <= 0) { eventWidget.append(this.NoRecordFound("No Event this week")); return; }
    for (var i = 0; i < dataObj.length; i++) { widget = $(this.GetEventWidgetHTML(dataObj[i])); eventWidget.append(widget); };
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
            officeDetail.GetEventList(dataObj.events);
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
        "startDate": moment().startOf('week').isoWeekday(4).add(officeDetail.options.addWeek, "week"),
        "endDate": moment().endOf('week').isoWeekday(4).add(officeDetail.options.addWeek, "week"),
        ranges: {
            'Current Week': [moment().startOf('week').isoWeekday(4).add(officeDetail.options.addWeek, "week"), moment().endOf('week').isoWeekday(4).add(officeDetail.options.addWeek, "week")],
            'Last 7 Days': [moment().startOf('week').isoWeekday(4).subtract(officeDetail.options.subtractWeek, "week"), moment().endOf('week').isoWeekday(4).subtract(officeDetail.options.subtractWeek, "week")],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, officeDetail.UpdateGlobalTimePeriodSelection).off("apply.daterangepicker").on('apply.daterangepicker', function (ev, picker) { officeDetail.UpdatePageDataByFilter(); });
    officeDetail.UpdateGlobalTimePeriodSelection(moment().startOf('week').isoWeekday(4).add(officeDetail.options.addWeek, "week"), moment().endOf('week').isoWeekday(4).add(officeDetail.options.addWeek, "week"));
    officeDetail.UpdatePageDataByFilter();
    officeDetail.AttachScrollBar($(".eventWidget, .audienceWidget, .taskWidget"));
};
officeDetail.DoPageSetting = function () { officeDetail.LoadGlobalTimeFilter(); };
