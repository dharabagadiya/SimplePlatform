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
office.options = {
    EditViewURL: "/Offices/Edit/",
    UpdateURL: "/Offices/Update",
    DeleteURL: "/Offices/Delete"
};
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
    sb.append("<p class=\"strong mb0\">Fundraising <span class=\"text-muted pull-right\">" + obj.Fundraising.ActTotal + " of " + obj.Fundraising.Total + " Total</span></p>");
    sb.append("<div class=\"progress animated-bar flat transparent progress-bar-xs mb10 mt0\"><div class=\"progress-bar progress-bar-blue\" role=\"progressbar\" data-transitiongoal=\"63\" aria-valuenow=\"63\" style=\"width: " + obj.Fundraising.ActTotal + "%;\"></div></div>");
    sb.append("<div class=\"row\">");
    sb.append("<div class=\"col-md-6\"><div class='divWidgetDetail'>Task <span class=\"pull-right\"><span class=\"badge\">" + obj.Task.ActTotal + "</span> of " + obj.Task.Total + "</span></div></div>");
    sb.append("<div class=\"col-md-6\"><div class='divWidgetDetail'>Events <span class=\"pull-right\"><span class=\"badge\">" + obj.Events.ActTotal + "</span> of " + obj.Events.Total + "</span></div></div>");
    sb.append("</div>");
    sb.append("<div class=\"row\">");
    sb.append("<div class=\"col-md-6\"><div class='divWidgetDetail'>Booking <span class=\"pull-right\"><span class=\"badge\">" + obj.BookingInProcess.ActTotal + "</span> of " + obj.BookingInProcess.Total + "</span></div></div>");
    //sb.append("<div class=\"col-md-6\">Booking " + obj.BookingConfirm.ActTotal + " of " + obj.BookingConfirm.Total + "</div>");
    sb.append("</div>");
    sb.append("</div>");
    sb.append("</div>");
    sb.append("</div>");
    sb.append("</div>");
    return sb.toString();
}
function BindOfficeWidgetClick(obj) {
    obj.find(".panel-close").off("click").on("click", function () {
        //alert("delete");
        office.DeletUserDetail(obj);
    });
}
office.OfficeWidget = function () {
    $(".OfficeWidget").empty();
    for (var i = 0; i < data.length; i++) {
        var widget = $(this.GetOfficeWidgetHTML(data[i])).data("office_detail", data[i].Id);
        //widget.data("office_detail", data[i].Id);
        BindOfficeWidgetClick(widget);
        $(".OfficeWidget").append(widget);
    };
}
office.DeletUserDetail = function (obj) {
    debugger
    var currentObj = obj;
    var officeDetail = obj.data("office_detail");
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        url: office.options.DeleteURL,
        async: false,
        data: JSON.stringify({ "id": officeDetail }),
        success: function (data) {
            var status = data;
            if (status) {
                //$('#myDataTable').dataTable().api().ajax.reload(null, false);
                office.OfficeWidget();
            } else {
            }
        }
    });
};

$(document).ready(function () {
    office.OfficeWidget();
});