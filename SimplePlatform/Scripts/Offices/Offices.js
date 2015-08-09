var office = {};
office.options = {
    EditViewURL: "/Offices/Edit/",
    UpdateURL: "/Offices/Update",
    DeleteURL: "/Offices/Delete",
    GetOffices: "/Offices/GetOffices",
    pageSize: 3,
    totalPageSize: 10,
    currentPage: 1,
    totalRecords: 10
};

office.GetOfficeGridPagination = function (obj) {
    obj.find("ul").bootstrapPaginator({
        currentPage: office.options.currentPage,
        totalPages: office.options.totalPageSize,
        bootstrapMajorVersion: 3,
        onPageChanged: function (e, oldPage, newPage) { office.GetOfficesData(newPage, office.options.pageSize); }
    });
};
office.GetOfficeWidgetHTML = function (obj) {
    var sb = new StringBuilder();
    sb.append("<div class=\"col-lg-3 col-md-6 col-sm-6 col-xs-12\">");
    sb.append("<div class=\"panel panel-info tile panelClose panelRefresh\" id=\"dyn_0\">");
    sb.append("<div class=\"panel-heading\">");
    sb.append("<h4 class=\"panel-title\">" + obj.Name + "</h4>");
    sb.append("<div class=\"panel-controls panel-controls-right\"><a class=\"panel-edit\"><i class=\"fa fa-edit\"></i></a><a class=\"panel-close\"><i class=\"fa fa-times\"></i></a></div>");
    sb.append("</div>");
    sb.append("<div class=\"panel-body pt0\">");
    sb.append("<div class=\"progressbar-stats-1\">");
    sb.append("<p class=\"strong mb0\">Fundraising <span class=\"text-muted pull-right\">" + obj.Fundraising.ActTotal + " of " + obj.Fundraising.Total + " Total</span></p>");
    sb.append("<div class=\"progress animated-bar flat transparent progress-bar-xs mb10 mt0\"><div class=\"progress-bar progress-bar-white\" role=\"progressbar\" data-transitiongoal=\"" + (obj.Fundraising.ActTotal * 10) + "\"></div></div>");
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
office.BindOfficeWidgetClick = function (obj) {
    obj.find(".panel-close").off("click").on("click", function () {
        //alert("delete");
        office.DeletUserDetail(obj);
    });
    obj.find(".panel-edit").off("click").on("click", function () { office.EditOfficeDetail(obj); });
}
office.OfficeWidget = function (dataObj) {
    if (IsNullOrEmpty(dataObj) || dataObj.length <= 0) { return; }
    $(".OfficeWidget").empty();
    for (var i = 0; i < dataObj.length; i++) {
        var widget = $(this.GetOfficeWidgetHTML(dataObj[i])).data("office_detail", dataObj[i]);
        this.BindOfficeWidgetClick(widget);
        $(".OfficeWidget").append(widget);
    };
    $(".OfficeWidget").find('.animated-bar .progress-bar').waypoint(function (direction) {
        $(this).progressbar({ display_text: 'none' });
    }, { offset: 'bottom-in-view' });
};
office.ValidateModalOfficeForm = function (obj) {
    obj.find("form")
    .bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            name: {
                message: 'The name is not valid',
                validators: {
                    notEmpty: {
                        message: 'The name is required and cannot be empty'
                    },
                    stringLength: {
                        min: 5,
                        max: 30,
                        message: 'The name must be more than 5 and less than 30 characters long'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_]+$/,
                        message: 'The name can contain a-z, A-Z, 0-9, or (_) only'
                    }
                }
            },
            contactNo: {
                message: 'The Contact No is not valid',
                validators: {
                    notEmpty: {
                        message: 'The Contact No is required and cannot be empty'
                    },
                    stringLength: {
                        min: 10,
                        max: 10,
                        message: 'The Contact No must be 10 characters long'
                    },
                    regexp: {
                        regexp: /^[1-9][0-9]{0,15}$/,
                        message: 'The city can contain 0-9 only'
                    }
                }
            },
            city: {
                message: 'The city is not valid',
                validators: {
                    notEmpty: {
                        message: 'The city is required and cannot be empty'
                    },
                    stringLength: {
                        min: 3,
                        max: 30,
                        message: 'The city must be more than 3 and less than 30 characters long'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_]+$/,
                        message: 'The city can contain a-z, A-Z, 0-9, or (_) only'
                    }
                }
            }
        }
    }).on('success.form.bv', function (e) {
        e.preventDefault();
        var formObj = $(e.target);
        var id = formObj.find("#txtOfficeID").val();
        var name = formObj.find("#txtName").val();
        var contactNo = formObj.find("#txtContactNo").val();
        var city = formObj.find("#txtCity").val();
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            url: office.options.UpdateURL,
            async: false,
            data: JSON.stringify({ "id": id, "name": name, "contactNo": contactNo, "city": city }),
            success: function (data) {
                var status = data;
                if (status) {
                    obj.modal('hide');
                } else {
                    obj.find("#divCommonMessage").removeClass("hidden");
                }
            }
        });
    });
};
office.EditOfficeDetail = function (obj) {
    var currentObj = obj;
    var officeDetail = obj.data("office_detail");
    $("#divCommonModalPlaceHolder").empty();
    ShowDialogBox($("#divCommonModalPlaceHolder"), (office.options.EditViewURL + officeDetail.ID), null, $.proxy(function (event, dialogContentPlaceHolder) {
        this.ValidateModalOfficeForm(dialogContentPlaceHolder);
        dialogContentPlaceHolder.find("#divCommonMessage").addClass("hidden");
    }, this));
};
office.DeletUserDetail = function (obj) {
    var currentObj = obj;
    var officeDetail = obj.data("office_detail");
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        url: office.options.DeleteURL,
        async: false,
        data: JSON.stringify({ "id": officeDetail.ID }),
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
office.GetOfficesData = function (pageNo, pageSize) {
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        url: office.options.GetOffices,
        async: true,
        data: JSON.stringify({ "pageNo": pageNo, "pageSize": pageSize }),
        success: function (data) {
            office.options.totalPageSize = Math.ceil(data.totalRecord / data.pageSize);
            office.options.totalRecords = data.totalRecord;
            office.options.pageSize = data.pageSize;
            office.options.currentPage = data.currentPage;
            office.OfficeWidget(data.offices);
            office.GetOfficeGridPagination($(".divOfficesGridPaging"));
            $(".divOfficesGridPagingDetail").find(".dataTables_info").empty().append("Showing " + ((office.options.currentPage * office.options.pageSize) - office.options.pageSize + 1) + " to " + ((office.options.currentPage * office.options.pageSize)) + " of " + office.options.totalRecords + " entries");
        }
    });
};
$(document).ready(function () { office.GetOfficesData(1, office.options.pageSize); });