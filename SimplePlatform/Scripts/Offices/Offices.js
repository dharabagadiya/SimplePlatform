var data = [{
    "Id": 1,
    "Name": "NGO",
    "Fundraising": { "ActTotal": 5, "Total": 10 },
    "Task": { "ActTotal": 5, "Total": 100 },
    "Events": { "ActTotal": 5, "Total": 100 },
    "BookingInProcess": { "ActTotal": 5, "Total": 100 },
    "BookingConfirm": { "ActTotal": 5, "Total": 100 }
}, {
    "Id": 2,
    "Name": "NGO",
    "Fundraising": { "ActTotal": 5, "Total": 10 },
    "Task": { "ActTotal": 5, "Total": 100 },
    "Events": { "ActTotal": 5, "Total": 100 },
    "BookingInProcess": { "ActTotal": 5, "Total": 100 },
    "BookingConfirm": { "ActTotal": 5, "Total": 100 }
}, {
    "Id": 3,
    "Name": "NGO",
    "Fundraising": { "ActTotal": 5, "Total": 10 },
    "Task": { "ActTotal": 5, "Total": 100 },
    "Events": { "ActTotal": 5, "Total": 100 },
    "BookingInProcess": { "ActTotal": 5, "Total": 100 },
    "BookingConfirm": { "ActTotal": 5, "Total": 100 }
}]
var office = {};
office.GetOfficeWidgetHTML = function (obj) {
    var sb = new StringBuilder();
    sb.append("<div class=\"col-lg-3 col-md-6 col-sm-6 col-xs-12\">");
    sb.append("<div class=\"panel panel-info tile panelClose panelRefresh\" id=\"dyn_0\">");
    sb.append("<div class=\"panel-heading\">");
    sb.append("<h4 class=\"panel-title\">" + obj.Name + "</h4>");
    sb.append("<div class=\"panel-controls panel-controls-right\"><a href=\"#\" class=\"panel-close\"><i class=\"fa fa-times\"></i></a></div>");
    sb.append("</div>");
    sb.append("<div class=\"panel-body pt0\">");
    sb.append("<div class=\"progressbar-stats-1\">");
    sb.append("<p class=\"strong mb0\">Fundraising <span class=\"text-muted pull-right small\">" + obj.Fundraising.ActTotal + " of " + obj.Fundraising.Total + " Total</span></p>");
    sb.append("<div class=\"progress animated-bar flat transparent progress-bar-xs mb10 mt0\">");
    sb.append("<div class=\"progress-bar progress-bar-blue\" role=\"progressbar\" data-transitiongoal=\"63\" aria-valuenow=\"63\" style=\"width: " + obj.Fundraising.ActTotal + "%;\"></div>");
    sb.append("</div>");
    sb.append("<div class=\"row\">");
    sb.append("<div class=\"col-md-6\">Task <span class=\"text-muted pull-right small\">" + obj.Task.ActTotal + " of " + obj.Task.Total + " Total</span></div>");
    sb.append("<div class=\"col-md-6\">Events " + obj.Events.ActTotal + " of " + obj.Events.Total + "</div>");
    sb.append("</div>");
    sb.append("<div class=\"row\">");
    sb.append("<div class=\"col-md-6\">Booking in Process" + obj.BookingInProcess.ActTotal + " of " + obj.BookingInProcess.Total + "</div>");
    sb.append("<div class=\"col-md-6\">Booking Confirm" + obj.BookingConfirm.ActTotal + " of " + obj.BookingConfirm.Total + "</div>");
    sb.append("</div>");
    sb.append("</div>");
    sb.append("</div>");
    sb.append("</div>");
    sb.append("</div>");
    return sb.toString();
}

office.OfficeWidget = function () {
    var widgets = null;
    $(".OfficeWidget").empty();
    for (var i = 0; i < data.length; i++) {
        $(".OfficeWidget").append($(this.GetOfficeWidgetHTML(data[i])));
    };
}
$(document).ready(function () {
    office.OfficeWidget();
});