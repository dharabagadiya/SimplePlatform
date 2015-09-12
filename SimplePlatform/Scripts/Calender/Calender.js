var calendar = {};
calendar.options = {
    GetEvents: "/Calender/GetEvents",
    GetTaskDetailURL: function (id) { return ("/Calender/TaskDetail/" + id); },
    GetEventDetailURL: function (id) { return ("/Calender/EventDetail/" + id); }
};
calendar.LoadCalenderByMonth = function (start, end, timezone, callback) {
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        async: true,
        data: JSON.stringify({ start: start.toDate(), end: end.toDate() }),
        url: calendar.options.GetEvents,
        success: function (data) { callback(data); }
    });
};

calendar.GetTaskDetail = function (event) {
    ShowDialogBox($("#divCommonModalPlaceHolder"), calendar.options.GetTaskDetailURL(event.id), null, $.proxy(function (event, dialogContentPlaceHolder) { }, this));
};
calendar.GetEventDetail = function (event) {
    ShowDialogBox($("#divCommonModalPlaceHolder"), calendar.options.GetEventDetailURL(event.id), null, $.proxy(function (event, dialogContentPlaceHolder) { }, this));
};

calendar.GetCalenderEventDetail = function (event) {
    var eventType = event.type.toUpperCase();
    switch (eventType) {
        case "TASK":
            calendar.GetTaskDetail(event);
            break;
        case "EVENT":
            calendar.GetEventDetail(event);
            break;
    }
};
calendar.DoCalenderSetting = function () {
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month'
        },
        defaultDate: new Date(),
        editable: false,
        eventLimit: 5,
        eventRender: function (event, element) {
            var contentObj = element.find(".fc-content");
            var eventTilteObj = contentObj.find(".fc-title");
            eventTilteObj.addClass("pull-left");
            contentObj.prepend("<div class=\"avatar pull-left mr15\"><img class='img-responsive' src=\"" + event.imageURL + "\" alt=\"avatar\"></div>");
        },
        events: function (start, end, timezone, callback) { calendar.LoadCalenderByMonth(start, end, timezone, callback); },
        eventClick: function (calEvent, jsEvent, view) {
            calendar.GetCalenderEventDetail(calEvent);
        }
    });
};
$(document).ready(function () {
    calendar.DoCalenderSetting();
});