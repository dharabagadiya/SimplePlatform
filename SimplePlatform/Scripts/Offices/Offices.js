var office = {};
office.jqXHRData = null;
office.options = {
    EditViewURL: "/Offices/Edit/",
    UpdateURL: "/Offices/Update",
    DeleteURL: "/Offices/Delete",
    GetOffices: "/Offices/GetOffices",
    OfficeDetailPage: function (id) { return ("/Offices/Detail/" + id); },
    pageSize: 9,
    totalPageSize: 10,
    currentPage: 1,
    totalRecords: 10,
    isEditDeleteEnable: false,
    startDate: null,
    endDate: null,
    addWeek: ((new Date().getDay()) <= 4 ? 0 : 1),
    subtractWeek: ((new Date().getDay()) <= 4 ? 1 : 0)
};
office.NoOfficeRecordFound = function () {
    $(".divOfficesGridPagingDetail").hide();
    $(".divOfficesGridPaging").hide();
    $(".OfficeWidget").empty().append("<div class=\"row\"><div class=\"col-md-12 text-center\">No offices found.</div></div>");
};
office.GetOfficeGridPagination = function (obj) {
    obj.show().find("ul").bootstrapPaginator({
        currentPage: office.options.currentPage,
        totalPages: office.options.totalPageSize,
        bootstrapMajorVersion: 3,
        onPageChanged: function (e, oldPage, newPage) { office.GetOfficesData(newPage, office.options.pageSize); }
    });
};
office.GetOfficeWidgetHTML = function (obj) {
    var sb = new StringBuilder();
    sb.append("<div class=\"col-lg-4 col-md-6 col-sm-6 col-xs-12 CP\">");
    sb.append("<div class=\"panel panel-info tile panelClose panelRefresh\" id=\"dyn_0\">");
    sb.append("<div class=\"panel-heading\">");
    sb.append("<h4 class=\"panel-title\">" + obj.Name + "</h4>");
    if (office.options.isEditDeleteEnable) { sb.append("<div class=\"panel-controls panel-controls-right\"><a class=\"panel-edit\"><i class=\"fa fa-edit\"></i></a><a class=\"panel-close\"><i class=\"fa fa-times\"></i></a></div>"); }
    sb.append("</div>");
    sb.append("<div class=\"panel-body pt0\">");
    sb.append("<div class=\"progressbar-stats-1\">");
    sb.append("<p class=\"strong mb0\">Fundraising <span class=\"text-muted pull-right\">" + obj.Fundraising.ActTotal + " of " + obj.Fundraising.Total + " Total</span></p>");
    sb.append("<div class=\"progress animated-bar flat transparent progress-bar-xs mb10 mt0\"><div class=\"progress-bar progress-bar-white\" role=\"progressbar\" data-transitiongoal=\"" + ((obj.Fundraising.ActTotal / obj.Fundraising.Total) * 100) + "\"></div></div>");
    if (IsNullOrEmpty(obj.ProfilePics)) {
        sb.append("<div class=\"col-md-5\">");
        sb.append("<img src=\"" + obj.ProfilePic + "\" alt=\"\" height=\"125px\" weight=\"185px\" class=\"img-responsive\"/>");
        sb.append("</div>");
    }
    sb.append("<div class=\"col-md-7\">");
    sb.append("<div class=\"row\">");
    sb.append("<div class=\"col-md-12\"><div class='divWidgetDetail'>Task <span class=\"pull-right\"><span class=\"badge\">" + obj.Task.ActTotal + "</span> of " + obj.Task.Total + "</span></div></div>");
    sb.append("<div class=\"col-md-12\"><div class='divWidgetDetail'>Arrival <span class=\"pull-right\"><span class=\"badge\">" + obj.Arrival.ActTotal + "</span> of " + obj.Arrival.Total + "</span></div></div>");
    sb.append("</div>");
    sb.append("<div class=\"row\">");
    sb.append("<div class=\"col-md-12\"><div class='divWidgetDetail'>Booking <span class=\"pull-right\"><span class=\"badge\">" + obj.BookingInProcess.ActTotal + "</span> of " + obj.BookingInProcess.Total + "</span></div></div>");
    sb.append("<div class=\"col-md-12\"><div class='divWidgetDetail'>Events <span class=\"pull-right\">" + obj.Events.Total + "</span></div></div>");
    sb.append("</div>");
    sb.append("</div>");
    sb.append("</div>");
    sb.append("</div>");
    sb.append("</div>");
    sb.append("</div>");
    return sb.toString();
}
office.BindOfficeWidgetClick = function (obj) {
    obj.find(".panel-close").off("click.panel-close").on("click.panel-close", function (event) { event.stopPropagation(); office.DeleteOfficeDetail(obj); });
    obj.find(".panel-edit").off("click.panel-edit").on("click.panel-edit", function (event) { event.stopPropagation(); office.EditOfficeDetail(obj); });
    obj.off("click.office_widget").on("click.office_widget", function () {
        var currentObj = $(this);
        var officeDetail = currentObj.data("office_detail")
        window.location.href = office.options.OfficeDetailPage(officeDetail.ID);
    });
}
office.OfficeWidget = function (dataObj) {
    if (IsNullOrEmpty(dataObj) || dataObj.length <= 0) { this.NoOfficeRecordFound(); return; }
    $(".OfficeWidget").empty();
    for (var i = 0; i < dataObj.length; i++) {
        var widget = $(this.GetOfficeWidgetHTML(dataObj[i])).data("office_detail", dataObj[i]);
        this.BindOfficeWidgetClick(widget);
        $(".OfficeWidget").append(widget);
    };
    this.GetOfficeGridPagination($(".divOfficesGridPaging"));
    $(".divOfficesGridPagingDetail").show().find(".dataTables_info").show().empty().append("Showing " + ((office.options.currentPage * office.options.pageSize) - office.options.pageSize + 1) + " to " + ((office.options.currentPage * office.options.pageSize) > office.options.totalRecords ? office.options.totalRecords : (office.options.currentPage * office.options.pageSize)) + " of " + office.options.totalRecords + " entries");
    $(".OfficeWidget").find('.animated-bar .progress-bar').waypoint(function (direction) { $(this).progressbar({ display_text: 'none' }); }, { offset: 'bottom-in-view' });
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
                        regexp: /^[a-zA-Z0-9_ ]+$/,
                        message: 'The name can contain a-z, A-Z, 0-9'
                    }
                }
            },
            contactNo: {
                message: 'The Contact is not valid',
                validators: {
                    notEmpty: {
                        message: 'The Contact is required and cannot be empty'
                    },
                    stringLength: {
                        min: 5,
                        max: 50,
                        message: 'The Contact must be more than 5 and less than 50 characters long'
                    },
                    //regexp: {
                    //    regexp: /^[1-9][0-9]{0,15}$/,
                    //    message: 'The city can contain 0-9 only'
                    //}
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
                        regexp: /^[a-zA-Z0-9_ ]+$/,
                        message: 'The city can contain a-z, A-Z, 0-9'
                    }
                }
            },
            image: {
                message: 'The selected file is not valid',
                validators: {
                    file: {
                        extension: 'jpeg,png,jpg,gif',
                        type: 'image/jpeg,image/png,image/jpg,image/gif',
                        maxSize: 2097152,   // 2048 * 1024
                        message: 'The selected file is not valid'
                    }
                }
            },
            ddlUser: {
                validators: {
                    notEmpty: {
                        message: 'Please select user.'
                    }
                }
            }
        }
    }).off('success.form.bv').on('success.form.bv', function (e) {
        e.preventDefault();
        var formObj = $(e.target);
        var id = formObj.find("#txtOfficeID").val();
        var name = formObj.find("#txtName").val();
        var contactNo = formObj.find("#txtContactNo").val();
        var city = formObj.find("#txtCity").val();
        var userID = formObj.find("#dwnUserID").val();
        $('#myFile').fileupload("option", {
            formData: { "id": id, "name": name, "contactNo": contactNo, "city": city, "userID": userID },
            done: function (e, data) {
                var status = data.result;
                if (status) {
                    obj.modal('hide');
                    ShowUpdateSuccessSaveAlert();
                } else {
                    obj.find("#divCommonMessage").removeClass("hidden");
                }
            }
        });
        if (office.jqXHRData)
            office.jqXHRData.submit();
        else
            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                url: office.options.UpdateURL,
                async: false,
                data: JSON.stringify({ "id": id, "name": name, "contactNo": contactNo, "city": city, "userID": userID }),
                success: function (data) {
                    var status = data;
                    if (status) {
                        obj.modal('hide');
                        ShowUpdateSuccessSaveAlert();
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
    ShowDialogBox($("#divCommonModalPlaceHolder"), (office.options.EditViewURL + officeDetail.ID), null, function (event, dialogContentPlaceHolder) {
        office.ValidateModalOfficeForm(dialogContentPlaceHolder);
        $('#myFile').fileupload({
            url: '/Offices/UpdateFile',
            dataType: 'json',
            add: function (e, data) {
                office.jqXHRData = data;
            }
        });
        $("#myFile").on('change', function () {
            $("#txtFileName").val(this.files[0].name);
        });
        dialogContentPlaceHolder.find("#dwnUserID").chosen({ width: "100%" });
        dialogContentPlaceHolder.find("#dwnUserID").val(dialogContentPlaceHolder.find("#txtUserID").val().split(",")).change().trigger("chosen:updated");
        dialogContentPlaceHolder.find("#divCommonMessage").addClass("hidden");
    });
};
office.DeleteOfficeDetail = function (obj) {
    var currentObj = obj;
    var officeDetail = obj.data("office_detail");
    ShowOkCancelDialogBox($("#divCommonModalPlaceHolder"), "Delete", "Are you sure you want to delete record?", function (event, dataModalPlaceHolder) {
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
                    office.ReloadOfficeCurrentPageData();
                } else {
                }
            }
        });
    }, function (event, dataModalPlaceHolder) { });
};
office.GetOfficesData = function (pageNo, pageSize) {
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        url: office.options.GetOffices,
        async: true,
        data: JSON.stringify({ "pageNo": pageNo, "pageSize": pageSize, startDate: office.options.startDate, endDate: office.options.endDate }),
        success: function (data) {
            office.options.totalPageSize = Math.ceil(data.totalRecord / data.pageSize);
            office.options.totalRecords = data.totalRecord;
            office.options.pageSize = data.pageSize;
            office.options.currentPage = data.currentPage;
            office.OfficeWidget(data.offices);
        }
    });
};
office.UpdateGlobalTimePeriodSelection = function (start, end) {
    office.options.startDate = start.toDate();
    office.options.endDate = end.toDate();
    $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
};
office.LoadGlobalTimeFilter = function () {
    $('#reportrange').daterangepicker({
        "startDate": moment().startOf('week').isoWeekday(4).add(office.options.addWeek, "week"),
        "endDate": moment().endOf('week').isoWeekday(4).add(office.options.addWeek, "week"),
        ranges: {
            'Current Week': [moment().startOf('week').isoWeekday(4).add(office.options.addWeek, "week"), moment().endOf('week').isoWeekday(4).add(office.options.addWeek, "week")],
            'Last 7 Days': [moment().startOf('week').isoWeekday(4).subtract(office.options.subtractWeek, "week"), moment().endOf('week').isoWeekday(4).subtract(office.options.subtractWeek, "week")],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, office.UpdateGlobalTimePeriodSelection).off("apply.daterangepicker").on('apply.daterangepicker', function (ev, picker) {
        office.GetOfficesData(office.options.currentPage, office.options.pageSize);
    });
    office.UpdateGlobalTimePeriodSelection(moment().startOf('week').isoWeekday(4).add(office.options.addWeek, "week"), moment().endOf('week').isoWeekday(4).add(office.options.addWeek, "week"));
    office.GetOfficesData(1, office.options.pageSize);
};
office.ReloadOfficeCurrentPageData = function () { this.GetOfficesData(this.options.currentPage, this.options.pageSize); }
$(document).ready(function () { office.LoadGlobalTimeFilter(); });