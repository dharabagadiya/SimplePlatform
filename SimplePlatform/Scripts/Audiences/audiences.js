var audiences = {};
audiences.options = {
    AddDataURL: "/Audiences/Add",
    UpdateDataURL: "/Audiences/Update",
    DeleteDataURL: "/Audiences/Delete",
    UpdateAttendStatusURL: "/Audiences/AttendStatus",
    AddFSMDetail: "/FSMDetail/Add",
    EditDataURL: function (id) { return ("/Audiences/Edit/" + id); },
    UpdateAudienceStatusURL: "/Audiences/UpdateAudienceStatus",
    startDate: null,
    endDate: null,
    addWeek: ((new Date().getDay()) <= 4 ? 0 : 1),
    subtractWeek: ((new Date().getDay()) <= 4 ? 1 : 0)
};
audiences.AddAudienceAjaxCall = function (obj, containerObj) {
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        url: audiences.options.AddDataURL,
        async: false,
        data: JSON.stringify(obj),
        success: function (data) {
            containerObj.find(".txtUserName").val("");
            containerObj.find(".txtContact").val("");
            containerObj.find(".txtContact").val("");
            containerObj.find(".dwnFSMList").val(0);
            containerObj.find(".dwnBookStatus").val(0);
            containerObj.find(".txtGSBAmount").val(0);
            containerObj.find(".txtDonationAmount").val(0);
        }
    });
};
audiences.SubmitBulkInsertForm = function (obj) {
    var ajaxSubmit = [];
    var formObj = $("#divAudienceBulkInsert");
    var name = formObj.find(".txtUserName").val();
    var visitDate = formObj.find(".txtVisitDate").val();
    var contact = formObj.find(".txtContact").val();
    var emailAddress = formObj.find(".txtEmailAddress").val();
    var visitType = parseInt(formObj.find("#hdnVisitType").val());
    //var fsmName = formObj.find(".txtFSMName").val();
    var fsmID = parseInt(formObj.find("#dwnFSMList").val());
    var placeID = parseInt(formObj.find("#hdnVisitPlaceID").val());
    var officeID = parseInt(formObj.find("#dwnOffices").val());
    var eventID = 0;
    var convensionID = 0;
    var serviceID = 0;
    var gsbAmount = formObj.find(".txtGSBAmount").val();
    var donationAmount = formObj.find(".txtDonationAmount").val();
    var bookingStatus = formObj.find(".dwnBookStatus").val();
    if (IsNullOrEmpty(gsbAmount)) { gsbAmount = 0; }
    if (IsNullOrEmpty(donationAmount)) { donationAmount = 0; }
    if (IsNullOrEmpty(bookingStatus)) { bookingStatus = 0; }
    if (IsNullOrEmpty(fsmID)) { fsmID = 0; }
    gsbAmount = parseFloat(gsbAmount);
    donationAmount = parseFloat(donationAmount);
    bookingStatus = parseFloat(bookingStatus);
    if (visitType == 2) { eventID = placeID; }
    else if (visitType == 3) { convensionID = placeID; }
    else if (visitType == 4) { serviceID = placeID; }
    if (!IsNullOrEmpty(name) || !IsNullOrEmpty(contact) || !(officeID == 0 && eventID == 0 && convensionID == 0)) {
        var dataObj = {
            name: name,
            visitDate: visitDate,
            contact: contact,
            emailAddress: emailAddress,
            visitType: visitType,
            officeID: officeID,
            eventID: eventID,
            convensionID: convensionID,
            serviceID: serviceID,
            fsmID: fsmID,
            bookingStatus: bookingStatus,
            gsbAmount: gsbAmount,
            donationAmount: donationAmount
        };
        ajaxSubmit.push(audiences.AddAudienceAjaxCall(dataObj, formObj));
    }
    $.when(ajaxSubmit).done(function () { window.location.reload(); });
};
audiences.ValidateModalAudienceQuickForm = function (obj) {
    obj.find("form")
    .bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            Name: {
                message: 'The name is not valid',
                validators: {
                    stringLength: {
                        min: 3,
                        max: 30,
                        message: 'The name must be more than 3 and less than 30 characters long'
                    },
                    regexp: {
                        regexp: /^['a-zA-Z0-9_ ]+$/,
                        message: 'The name can containe a-z, A-Z, 0-9, \',( ), or (_) only'
                    }
                }
            },
            emailAddress: {
                validators: {
                    notEmpty: {
                        message: 'The email address is required'
                    },
                    emailAddress: {
                        message: 'The input is not a valid email address'
                    }
                }
            },
            Contact: {
                message: 'The contact no is not valid',
                validators: {
                    stringLength: {
                        min: 8,
                        max: 20,
                        message: 'The contact no must be min 8 to 20 characters long'
                    },
                    regexp: {
                        regexp: /^[0-9 -]+$/,
                        message: 'The contact can contain 0-9, (-), or ( ) only'
                    }
                }
            },
            FSMName: {
                message: 'The FSM name is not valid',
                validators: {
                    stringLength: {
                        min: 3,
                        max: 15,
                        message: 'The FSM name must be more than 3 and less than 15 characters long'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_ ]+$/,
                        message: 'The FSM name can containe a-z, A-Z, 0-9, ( ), or (_) only'
                    }
                }
            }
        }
    }).off('success.form.bv').on('success.form.bv', function (e) {
        e.preventDefault();
        audiences.SubmitBulkInsertForm(obj);
    });
};
audiences.LoadQuickBooking = function () {
    var audienceDetailRow = $(".audienceDetailRow");
    for (var i = 1 ; i < 0; i++) { audienceDetailRow.after(audienceDetailRow.clone()); }
    $(".btnAddUsers").off("click.btnAddUsers").on("click.btnAddUsers", $.proxy(function () { }, this));
    $(".txtVisitDate").datepicker({ autoclose: true, todayHighlight: true });
    $(".dwnBookStatus").chosen({ width: "100%" });
    $(".dwnOffices").chosen({ width: "100%" });
    $(".dwnVisitType").each(function () {
        $(this).chosen({ width: "100%" }).off("change").on("change", function () {
            var rowObj = $(this).closest(".audienceDetailRow");
            var groupObj = $(this.options[this.selectedIndex]).closest('optgroup');
            var visitType = 0, placeID = 0;
            if (groupObj.length > 0) { visitType = groupObj.attr("id"); placeID = $(this).val(); } else { placeID = $(this).val(); }
            rowObj.find("#hdnVisitType").val(visitType);
            rowObj.find("#hdnVisitPlaceID").val(placeID);
            if (parseInt(visitType) == 1 || parseInt(visitType) == 4) { rowObj.find(".dwnBookStatus").parent().hide(); }
            else { rowObj.find(".dwnBookStatus").parent().show(); }
        }).change();
    });
    $(".dwnFSMList").chosen({ width: "100%" }).off("change").on("change", function () {
        var currentObj = $(this);
        var fsmID = parseInt(currentObj.val());
        if (fsmID == -1) { audiences.AddFSMDetailDialogBox(); }
    });
    audiences.ValidateModalAudienceQuickForm($("#divAudienceBulkInsert"));
};
audiences.ValidateModalFSMDetailForm = function (obj) {
    obj.find("form").bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            txtUserName: {
                message: 'The name is not valid',
                validators: {
                    notEmpty: {
                        message: 'The name is required and cannot be empty'
                    },
                    stringLength: {
                        min: 3,
                        max: 30,
                        message: 'The name must be more than 3 and less than 30 characters long'
                    },
                    regexp: {
                        regexp: /^['a-zA-Z0-9_ ]+$/,
                        message: 'The name can containe a-z, A-Z, 0-9, \',( ), or (_) only'
                    }
                }
            },
            emailAddress: {
                validators: {
                    notEmpty: {
                        message: 'The email address is required'
                    },
                    emailAddress: {
                        message: 'The input is not a valid email address'
                    }
                }
            },
            txtPhoneNumber: {
                message: 'The phone number is not valid',
                validators: {
                    stringLength: {
                        min: 8,
                        max: 20,
                        message: 'The phone number must be min 8 to 20 characters long'
                    },
                    notEmpty: {
                        message: 'The phone number is required'
                    },
                    regexp: {
                        regexp: /^[0-9]+$/,
                        message: 'The phone number can contain 0-9, or ( ) only'
                    }
                }
            }
        }
    }).off('success.form.bv').on('success.form.bv', function (e) {
        e.preventDefault();
        var formObj = $(e.target);;
        var name = formObj.find("#txtUserName").val();
        var emailID = formObj.find("#txtUserEmailAddress").val();
        var phoneNumber = formObj.find("#txtPhoneNumber").val();
        var dataObj = {
            name: name,
            emailID: emailID,
            phoneNumber: phoneNumber
        };
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            url: audiences.options.AddFSMDetail,
            data: JSON.stringify(dataObj),
            success: function (data) {
                var status = data;
                if (status != 0) {
                    obj.modal('hide');
                    $(".dwnFSMList").append("<option value='" + status + "' >" + name + "</option>");
                    $(".dwnFSMList").trigger("chosen:updated");
                } else {
                    obj.find("#divCommonMessage").removeClass("hidden");
                }
            }
        });
    });
};
audiences.AddFSMDetailDialogBox = function () {
    $("#divCommonModalPlaceHolder").empty();
    ShowDialogBox($("#divCommonModalPlaceHolder"), audiences.options.AddFSMDetail, null, function (event, dialogContentPlaceHolder) {
        audiences.ValidateModalFSMDetailForm(dialogContentPlaceHolder);
        dialogContentPlaceHolder.find("#divCommonMessage").addClass("hidden");
    });
};
audiences.ValidateModalAudienceForm = function (obj) {
    obj.find("form")
    .bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            Name: {
                message: 'The name is not valid',
                validators: {
                    notEmpty: {
                        message: 'The name is required and cannot be empty'
                    },
                    stringLength: {
                        min: 3,
                        max: 30,
                        message: 'The name must be more than 3 and less than 30 characters long'
                    },
                    regexp: {
                        regexp: /^['a-zA-Z0-9_ ]+$/,
                        message: 'The name can containe a-z, A-Z, 0-9, \',( ), or (_) only'
                    }
                }
            },
            emailAddress: {
                validators: {
                    notEmpty: {
                        message: 'The email address is required'
                    },
                    emailAddress: {
                        message: 'The input is not a valid email address'
                    }
                }
            },
            Contact: {
                message: 'The contact no is not valid',
                validators: {
                    stringLength: {
                        min: 8,
                        max: 20,
                        message: 'The contact no must be min 8 to 20 characters long'
                    },
                    regexp: {
                        regexp: /^[0-9 -]+$/,
                        message: 'The contact can contain 0-9, (-), or ( ) only'
                    }
                }
            },
            FSMName: {
                message: 'The FSM name is not valid',
                validators: {
                    stringLength: {
                        min: 3,
                        max: 15,
                        message: 'The FSM name must be more than 3 and less than 15 characters long'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_ ]+$/,
                        message: 'The FSM name can containe a-z, A-Z, 0-9, ( ), or (_) only'
                    }
                }
            }
        }
    }).off('success.form.bv').on('success.form.bv', function (e) {
        e.preventDefault();
        var formObj = $(e.target);
        var audienceID = formObj.find("#hdnAudienceID").val();
        var name = formObj.find("#txtName").val();
        var contact = formObj.find("#txtContact").val();
        var emailAddress = formObj.find(".txtEmailAddress").val();
        var visitDate = formObj.find(".txtVisitDate").val();
        var visitTypeID = formObj.find("#dwnPeopleVistiType").val();
        var officeID = formObj.find("#dwnOffices").val();
        var eventID = formObj.find("#dwnEvetns").val();
        var conventionID = formObj.find("#dwnConvensions").val();
        //var fsmName = formObj.find(".txtFSMName").val();
        var serviceID = formObj.find("#dwnServices").val();
        var fsmID = formObj.find("#dwnFSMList").val();
        var bookingStatus = formObj.find("#dwnBookStatus").val();
        var gsbAmount = formObj.find("#txtGSBAmount").val();
        var donationAmount = formObj.find("#txtDonationAmount").val();
        if (IsNullOrEmpty(officeID) && officeID <= 0) { officeID = 0; }
        if (IsNullOrEmpty(eventID) && eventID <= 0) { eventID = 0; }
        if (IsNullOrEmpty(fsmID) && fsmID <= 0) { fsmID = 0; }
        if (IsNullOrEmpty(conventionID) && conventionID <= 0) { conventionID = 0; }
        if (IsNullOrEmpty(serviceID) && serviceID <= 0) { serviceID = 0; }
        if (IsNullOrEmpty(gsbAmount)) { gsbAmount = 0; }
        if (IsNullOrEmpty(donationAmount) && donationAmount <= 0) { donationAmount = 0; }
        if (visitTypeID == 1) { eventID = 0; conventionID = 0; }
        var dataObj = {
            audienceID: audienceID,
            name: name,
            visitDate: visitDate,
            contact: contact,
            emailAddress: emailAddress,
            visitType: visitTypeID,
            officeID: officeID,
            eventID: eventID,
            convensionID: conventionID,
            serviceID: serviceID,
            fsmID: fsmID,
            bookingStatus: bookingStatus,
            gsbAmount: gsbAmount,
            donationAmount: donationAmount
        };
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            url: audiences.options.UpdateDataURL,
            async: false,
            data: JSON.stringify(dataObj),
            success: function (data) {
                var status = data;
                if (status) {
                    obj.modal('hide');
                    audiences.LoadAudienceList();
                } else { }
            }
        });
    });
};
audiences.BindHeaderAddAudienceDropDownChangeEvent = function (obj) {
    obj.find("#dwnPeopleVistiType").off("change.dwnPeopleVistiType").on("change.dwnPeopleVistiType", function () {
        obj.find(".divVisitTypeControl").hide();
        obj.find(".divVisitTypeControl[data-id='" + $(this).val() + "']").show();
        if (parseInt($(this).val()) == 1) { obj.find(".ddlBookingStatus").hide(); }
        else { obj.find(".ddlBookingStatus").show(); }
    });
};
audiences.EditAudienceDetail = function (obj) {
    var audienceDetail = obj.data("audience_detail");
    $("#divCommonModalPlaceHolder").empty();
    ShowDialogBox($("#divCommonModalPlaceHolder"), audiences.options.EditDataURL(audienceDetail.ID), null, $.proxy(function (event, dialogContentPlaceHolder) {
        dialogContentPlaceHolder.find("#dwnPeopleVistiType").val(dialogContentPlaceHolder.find("#hdnVisitTypeID").val());
        dialogContentPlaceHolder.find("#dwnOffices").val(dialogContentPlaceHolder.find("#hdnOfficeID").val());
        dialogContentPlaceHolder.find("#dwnEvetns").val(dialogContentPlaceHolder.find("#hdnEventID").val());
        dialogContentPlaceHolder.find("#dwnConvensions").val(dialogContentPlaceHolder.find("#hdnConventionID").val());
        dialogContentPlaceHolder.find("#dwnServices").val(dialogContentPlaceHolder.find("#hdnServiceID").val());
        dialogContentPlaceHolder.find("#dwnFSMList").val(dialogContentPlaceHolder.find("#hdnFSMID").val());
        dialogContentPlaceHolder.find("#dwnBookStatus").val(dialogContentPlaceHolder.find("#hdnBookinStatus").val());
        dialogContentPlaceHolder.find(".txtVisitDate").datepicker({ autoclose: true, todayHighlight: true });
        audiences.BindHeaderAddAudienceDropDownChangeEvent(dialogContentPlaceHolder);
        dialogContentPlaceHolder.find("#dwnPeopleVistiType").val(dialogContentPlaceHolder.find("#hdnVisitTypeID").val()).change();
        audiences.ValidateModalAudienceForm(dialogContentPlaceHolder);
    }, this));
};
audiences.DeletAudienceDetail = function (obj) {
    var currentObj = obj;
    var audienceDetail = obj.data("audience_detail");
    ShowOkCancelDialogBox($("#divCommonModalPlaceHolder"), "Delete", "Are you sure you want to delete record?", function (event, dataModalPlaceHolder) {
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            url: audiences.options.DeleteDataURL,
            async: false,
            data: JSON.stringify({ "id": audienceDetail.ID }),
            success: function (data) {
                var status = data;
                if (status) {
                    audiences.LoadAudienceList();
                } else {
                }
            }
        });
    }, function (event, dataModalPlaceHolder) { });
};
audiences.UpdateAudienceAttendStatus = function (audienceID, status) {
    $("#divCommonModalPlaceHolder").empty();
    ShowDialogBox($("#divCommonModalPlaceHolder"), audiences.options.UpdateAudienceStatusURL, null, $.proxy(function (event, dialogContentPlaceHolder) {
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
                url: audiences.options.UpdateAttendStatusURL,
                success: function (data) {
                    dialogContentPlaceHolder.modal("hide");
                    audiences.LoadAudienceList();
                }
            });
        });
    }, this));
};
audiences.LoadAudienceList = function () {
    $('#audienceList').dataTable().fnDestroy();
    $('#audienceList').dataTable({
        renderer: {
            "header": "bootstrap",
            "pageButton": "bootstrap"
        },
        "ajax": {
            "url": "/Audiences/GetAudiences",
            "type": "POST",
            "data": { startDate: audiences.options.startDate.toDateString(), endDate: audiences.options.endDate.toDateString() }
        },
        "displayLength": 25,
        responsive: true,
        "deferRender": true,
        "columns": [
            { "data": "VisitDate" },
            { "data": "Name" },
            { "data": "Contact" },
            { "data": "EmailAddress" },
            { "data": "VisitType" },
            { "data": "EventName" },
            { "data": "ConventionName" },
            { "data": "ServiceName" },
            { "data": "FSMName" },
            { "data": "Status", "width": '5%' },
            { "data": "GSBAmount", "width": '5%' },
            { "data": "DonationAmount", "width": '5%' },
            {
                "data": null,
                "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                    var currentObj = $(cell);
                    currentObj.css({ "text-align": "center" }).data("audience_detail", rowData);
                    currentObj.off("click.dataTableEditLink").on("click.dataTableEditLink", function () { audiences.EditAudienceDetail($(this)); });
                },
                render: function (o) { return '<a href="#"><i class="ui-tooltip fa fa-pencil" style="font-size: 22px;" data-original-title="Edit"></i></a>'; },
                "orderable": false,
                "width": '2%'
            }, {
                "data": null,
                "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                    var currentObj = $(cell);
                    currentObj.css({ "text-align": "center" }).data("audience_detail", rowData);
                    currentObj.off("click.dataTableDeleteLink").on("click.dataTableDeleteLink", function () { audiences.DeletAudienceDetail($(this)); });
                },
                render: function (o) { return '<a href="#"><i class="ui-tooltip fa fa-trash-o" style="font-size: 22px;" data-original-title="Delete"></i></a>'; },
                "orderable": false,
                "width": '2%'
            }, {
                "data": null,
                "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                    var currentObj = $(cell);
                    currentObj.css({ "text-align": "center" }).data("task_detail", rowData);
                    currentObj.off("click.updateStatus").on("click.updateStatus", function () {
                        audiences.UpdateAudienceAttendStatus(rowData.ID, rowData.Attended);
                    });
                },
                render: function (o) {
                    if (o.VisitType.toUpperCase() == "OFFICE") { return "-" }
                    return "<a href=\"#\">" + (o.Attended ? "<i class=\"icon ion-android-checkbox-outline\" style=\"font-size: 22px;\"></i>" : "<i class=\"icon ion-android-checkbox-outline-blank\" style=\"font-size: 22px;\"></i>") + "</a>";
                },
                "orderable": false,
                "width": '2%'
            }
        ]
    }).removeClass('display').addClass('table table-striped table-bordered');
};
audiences.UpdateGlobalTimePeriodSelection = function (start, end) {
    audiences.options.startDate = start.toDate();
    audiences.options.endDate = end.toDate();
    $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
};
audiences.LoadGlobalTimeFilter = function () {
    $('#reportrange').daterangepicker({
        "startDate": moment().startOf('week').isoWeekday(4).add(audiences.options.addWeek, "week"),
        "endDate": moment().endOf('week').isoWeekday(4).add(audiences.options.addWeek, "week"),
        ranges: {
            'Current Week': [moment().startOf('week').isoWeekday(4).add(audiences.options.addWeek, "week"), moment().endOf('week').isoWeekday(4).add(audiences.options.addWeek, "week")],
            'Last 7 Days': [moment().startOf('week').isoWeekday(4).subtract(audiences.options.subtractWeek, "week"), moment().endOf('week').isoWeekday(4).subtract(audiences.options.subtractWeek, "week")],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, audiences.UpdateGlobalTimePeriodSelection).off("apply.daterangepicker").on('apply.daterangepicker', function (ev, picker) {
        audiences.LoadAudienceList();
    });
    audiences.UpdateGlobalTimePeriodSelection(moment().startOf('week').isoWeekday(4).add(audiences.options.addWeek, "week"), moment().endOf('week').isoWeekday(4).add(audiences.options.addWeek, "week"));
    audiences.LoadAudienceList();
};
audiences.DoPageSetting = function () {
    audiences.LoadGlobalTimeFilter();
    audiences.LoadQuickBooking();
};
