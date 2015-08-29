var calendar = {};
calendar.options = {
    GetEvents: "/Calender/GetEvents"
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
calendar.DoCalenderSetting = function () {
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month'
        },
        defaultDate: new Date(),
        editable: false,
        eventLimit: true,
        eventRender: function (event, element) { },
        events: function (start, end, timezone, callback) { calendar.LoadCalenderByMonth(start, end, timezone, callback); }
    });
};
$(document).ready(function () {
    calendar.DoCalenderSetting();
});