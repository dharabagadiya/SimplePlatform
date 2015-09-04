var conventions = {};
conventions.jqXHRData = null;
conventions.filesList = [];
conventions.filesName = [];
conventions.options = {
    EditViewURL: "/Conventions/Edit/",
    UpdateURL: "/Conventions/Update",
    DeleteURL: "/Conventions/Delete",
    GetConventions: "/Conventions/GetConventions",
    UploadAttachment: "/Conventions/UploadAttachment/",
    pageSize: 9,
    totalPageSize: 10,
    currentPage: 1,
    totalRecords: 10,
    isEditDeleteEnable: false
};
conventions.ValidateModalConventionForm = function (obj) {
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
                            message: 'The name can contain a-z, A-Z, 0-9, (_), or (_) only'
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
                    ddlUser: {
                        validators: {
                            notEmpty: {
                                message: 'Please select user.'
                            }
                        }
                    }
                }
            }
        }).on('success.form.bv', function (e) {
            e.preventDefault();
            var formObj = $(e.target);;
            var name = formObj.find("#txtName").val();
            var dates = formObj.find("#datetimerange").val().split('-');
            var startDates = dates[0].trim();
            var endDates = dates[1].trim();
            var description = formObj.find("#txtDescription").val();
            var userID = 0;//formObj.find("#dwnUserId").val();
            var conventionID = formObj.find("#hdnConventionID").val();
            var city = formObj.find("#txtCity").val();
            $('#frmConventionEdit').fileupload("option", {
                formData: { "name": name, "startDate": startDates, "endDate": endDates, "description": description, "userID": userID, "conventionID": conventionID, "city": city },
                done: function (data) {
                    var status = data;
                    if (status) {
                        obj.modal('hide');
                        ShowUpdateSuccessSaveAlert();
                    } else {
                        obj.find("#divCommonMessage").removeClass("hidden");
                    }
                }
            });
            if (conventions.jqXHRData)
                conventions.jqXHRData.submit();
            else
                $.ajax({
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    url: conventions.options.UpdateURL,
                    async: false,
                    data: JSON.stringify({ "name": name, "startDate": startDates, "endDate": endDates, "description": description, "userID": userID, "conventionID": conventionID, "city": city }),
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
conventions.EditConventionDetail = function (obj) {
    var currentObj = obj;
    var conventionDetail = obj.data("convention_detail");
    $("#divCommonModalPlaceHolder").empty();
    ShowDialogBox($("#divCommonModalPlaceHolder"), (conventions.options.EditViewURL + conventionDetail.id), null, $.proxy(function (event, dialogContentPlaceHolder) {
        this.ValidateModalConventionForm(dialogContentPlaceHolder);
        dialogContentPlaceHolder.find("#datetimerange").daterangepicker({ timePicker24Hour: true, timePicker: true, timePickerIncrement: 15, locale: { format: 'MM/DD/YYYY HH:mm' } });
        $('#frmConventionEdit').fileupload({
            url: '/Conventions/UpdateFile',
            dataType: 'json',
            add: function (e, data) {
                //simplePlatform.filesList.push(data.files[0]);
                //paramNames.push(data.fileInput[0].name);
                conventions.jqXHRData = data;
            }
        });
        $("#fuImage").on('change', function () {
            $("#fuImageName").val(this.files[0].name);
        });
        dialogContentPlaceHolder.find("#divCommonMessage").addClass("hidden");
    }, this));
};
conventions.DeletConventionDetail = function (obj) {
    var currentObj = obj;
    var conventionDetail = obj.data("convention_detail");
    ShowOkCancelDialogBox($("#divCommonModalPlaceHolder"), "Delete", "Are you sure you want to delete record?", function (event, dataModalPlaceHolder) {
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            url: conventions.options.DeleteURL,
            async: false,
            data: JSON.stringify({ "id": conventionDetail.id }),
            success: function (data) {
                var status = data;
                if (status) {
                    window.location.reload();
                } else {
                }
            }
        });
    }, function (event, dataModalPlaceHolder) { });
};
conventions.ValidateModalConventionUploadForm = function (obj) {
    obj.find("form")
        .bootstrapValidator({
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                fileImage: {
                    message: 'The selected file is not valid',
                    validators: {
                        notEmpty: {
                            message: 'The image file is required and cannot be empty'
                        },
                        file: {
                            extension: 'jpeg,png,jpg,gif',
                            type: 'image/jpeg,image/png,image/jpg,image/gif',
                            maxSize: 2097152,   // 2048 * 1024
                            message: 'The selected file is not valid'
                        }
                    }
                }
            }
        }).on('success.form.bv', function (e) {
            e.preventDefault();
            var formObj = $(e.target);
            var conventionID = formObj.find("#hdnConventionID").val();
            $('#frmConventionUpload').fileupload("option", {
                formData: { "conventionID": conventionID },
                done: function (data) {
                    var status = data;
                    if (status) {
                        obj.modal('hide');
                        ShowSuccessSaveAlert();
                    } else {
                        obj.find("#divCommonMessage").removeClass("hidden");
                    }
                }
            });
            $('#frmConventionUpload').fileupload('send', { files: conventions.filesList });
        });
};
conventions.UploadConventionDetail = function (obj) {
    var currentObj = obj;
    var conventionDetail = obj.data("convention_detail");
    $("#divCommonModalPlaceHolder").empty();
    ShowDialogBox($("#divCommonModalPlaceHolder"), (conventions.options.UploadAttachment + conventionDetail.id), null, $.proxy(function (event, dialogContentPlaceHolder) {
        this.ValidateModalConventionUploadForm(dialogContentPlaceHolder);
        $('#frmConventionUpload').fileupload({
            url: '/Conventions/UploadAttachment',
            dataType: 'json',
            add: function (e, data) {
                conventions.filesList.push(data.files[0]);
                conventions.filesName.push(data.files[0].name);
            }
        });
        $("#fuImage").on('change', function () {
            $("#fuImageName").val(conventions.filesName.join(','));
        });
        dialogContentPlaceHolder.find("#divCommonMessage").addClass("hidden");
    }, this));
};
conventions.BindConventionWidgetClick = function (obj) {
    obj.find(".panel-close").off("click.panel-close").on("click.panel-close", function (event) { event.stopPropagation(); conventions.DeletConventionDetail(obj); });
    obj.find(".panel-edit").off("click.panel-edit").on("click.panel-edit", function (event) { event.stopPropagation(); conventions.EditConventionDetail(obj); });
    obj.find(".panel-upload").off("click.panel-upload").on("click.panel-upload", function (event) { event.stopPropagation(); conventions.UploadConventionDetail(obj); });
}
conventions.GetConventionWidgetHTML = function (obj) {
    var sb = new StringBuilder();
    sb.append("<div class=\"col-lg-4 col-md-6 col-sm-6 col-xs-12 CP\">");
    sb.append("<div class=\"panel panel-info tile panelClose panelRefresh\" id=\"dyn_0\">");
    sb.append("<div class=\"panel-heading\">");
    sb.append("<h4 class=\"panel-title\">" + obj.Name + "</h4>");
    if (conventions.options.isEditDeleteEnable) {
        sb.append("<div class=\"panel-controls panel-controls-right\"><a class=\"panel-upload\"><i class=\"fa fa-upload\"></i></a><a class=\"panel-edit\"><i class=\"fa fa-edit\"></i></a><a class=\"panel-close\"><i class=\"fa fa-times\"></i></a></div>");
    }
    sb.append("</div>");
    sb.append("<div class=\"panel-body pt0\">");
    sb.append("<div class=\"row\">");
    if (IsNullOrEmpty(obj.ProfilePics)) {
        sb.append("<div class=\"col-md-5\">");
        sb.append("<img src=\"" + obj.ProfilePic + "\" alt=\"\" class=\"img-responsive\"/>");
        sb.append("</div>");
    }
    sb.append("<div class=\"col-md-7\">");
    sb.append("<div class=\"row\">");
    sb.append("<div class=\"col-md-12\"><div class='divWidgetDetail'>Total Booking <span class=\"pull-right\"><span class=\"badge\">" + obj.Booking.ActTotal + "</span></span></div></div>");
    sb.append("<div class=\"col-md-12\"><div class='divWidgetDetail'>Fund Raising <span class=\"pull-right\"><span class=\"badge\">" + obj.Donation.ActTotal + "</span></span></div></div>");
    sb.append("</div>");
    sb.append("<div class=\"row\">");
    sb.append("<div class=\"col-md-12\"><div class='divWidgetDetail'>GSB Amount <span class=\"pull-right\"><span class=\"badge\">" + obj.GSBAmount.ActTotal + "</span></span></div></div>");
    sb.append("<div class=\"col-md-12\"><div class='divWidgetDetail'>Events <span class=\"pull-right\"><span class=\"badge\">" + obj.Events.ActTotal + "</span></span></div></div>");
    sb.append("</div>");
    sb.append("</div>");
    sb.append("</div>");
    sb.append("</div>");
    sb.append("</div>");
    sb.append("</div>");
    return sb.toString();
}
conventions.GetConventionGridPagination = function (obj) {
    obj.show().find("ul").bootstrapPaginator({
        currentPage: conventions.options.currentPage,
        totalPages: conventions.options.totalPageSize,
        bootstrapMajorVersion: 3,
        onPageChanged: function (e, oldPage, newPage) { conventions.GetConventionsData(newPage, office.options.pageSize); }
    });
};
conventions.NoOfficeRecordFound = function () {
    $(".divConventionsGridPagingDetail").hide();
    $(".divConventionsGridPaging").hide();
    $(".conventionWidgets").empty().append("<div class=\"row\"><div class=\"col-md-12 text-center\">No offices found.</div></div>");
};
conventions.ConventionsWidget = function (dataObj) {
    if (IsNullOrEmpty(dataObj) || dataObj.length <= 0) { this.NoOfficeRecordFound(); return; }
    $(".conventionWidgets").empty();
    for (var i = 0; i < dataObj.length; i++) {
        var widget = $(this.GetConventionWidgetHTML(dataObj[i])).data("convention_detail", dataObj[i]);
        this.BindConventionWidgetClick(widget);
        $(".conventionWidgets").append(widget);
    };
    this.GetConventionGridPagination($(".divConventionsGridPaging"));
    $(".divConventionsGridPaging").show();
    $(".divConventionsGridPagingDetail").show().find(".dataTables_info").show().empty().append("Showing " + ((conventions.options.currentPage * conventions.options.pageSize) - conventions.options.pageSize + 1) + " to " + (((conventions.options.currentPage * conventions.options.pageSize) > conventions.options.totalRecords ? conventions.options.totalRecords : conventions.options.currentPage * conventions.options.pageSize)) + " of " + conventions.options.totalRecords + " entries");
    $(".conventionWidgets").find('.animated-bar .progress-bar').waypoint(function (direction) { $(this).progressbar({ display_text: 'none' }); }, { offset: 'bottom-in-view' });
};
conventions.GetConventionsData = function (pageNo, pageSize) {
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        url: conventions.options.GetConventions,
        async: true,
        data: JSON.stringify({ "pageNo": pageNo, "pageSize": pageSize }),
        success: function (data) {
            conventions.options.totalPageSize = Math.ceil(data.totalRecord / data.pageSize);
            conventions.options.totalRecords = data.totalRecord;
            conventions.options.pageSize = data.pageSize;
            conventions.options.currentPage = data.currentPage;
            conventions.ConventionsWidget(data.conventions);
        }
    });
};
conventions.ReloadConventionCurrentPageData = function () {
    this.GetConventionsData(this.options.currentPage, this.options.pageSize);
}
$(document).ready(function () { conventions.ReloadConventionCurrentPageData(); });