﻿var audiences = {};
audiences.options = {
    AddDataURL: "/Audiences/Add",
    UpdateDataURL: "/Audiences/Update",
    DeleteDataURL: "/Audiences/Delete",
    UpdateAttendStatusURL: "/Audiences/AttendStatus",
    EditDataURL: function (id) { return ("/Audiences/Edit/" + id); },
    startDate: null,
    endDate: null
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
    obj.find(".audienceDetailRow").each(function () {
        var formObj = $(this);
        var name = formObj.find(".txtUserName").val();
        var visitDate = formObj.find(".txtVisitDate").val();
        var contact = formObj.find(".txtContact").val();
        var visitType = parseInt(formObj.find("#hdnVisitType").val());
        var fsmName = formObj.find(".txtFSMName").val();
        //var fsmID = parseInt(formObj.find(".dwnFSMList").val());
        var placeID = parseInt(formObj.find("#hdnVisitPlaceID").val());
        var officeID = parseInt(formObj.find("#dwnOffices").val());
        var eventID = 0;
        var convensionID = 0;
        var gsbAmount = formObj.find(".txtGSBAmount").val();
        var donationAmount = formObj.find(".txtDonationAmount").val();
        var bookingStatus = formObj.find(".dwnBookStatus").val();
        if (IsNullOrEmpty(gsbAmount)) { gsbAmount = 0; }
        if (IsNullOrEmpty(donationAmount)) { donationAmount = 0; }
        if (IsNullOrEmpty(bookingStatus)) { bookingStatus = 0; }
        gsbAmount = parseFloat(gsbAmount);
        donationAmount = parseFloat(donationAmount);
        bookingStatus = parseFloat(bookingStatus);
        if (visitType == 2) { eventID = placeID; }
        else if (visitType == 3) { convensionID = placeID; }
        if (!IsNullOrEmpty(name) || !IsNullOrEmpty(contact) || !(officeID == 0 && eventID == 0 && convensionID == 0)) {
            var dataObj = {
                name: name,
                visitDate: visitDate,
                contact: contact,
                visitType: visitType,
                officeID: officeID,
                eventID: eventID,
                convensionID: convensionID,
                fsmName: fsmName,
                bookingStatus: bookingStatus,
                gsbAmount: gsbAmount,
                donationAmount: donationAmount
            };
            ajaxSubmit.push(audiences.AddAudienceAjaxCall(dataObj, formObj));
        }
    });
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
            Contact: {
                message: 'The email address/contact no is not valid',
                validators: {
                    stringLength: {
                        min: 8,
                        max: 20,
                        message: 'The email address/contact no must be min 8 to 20 characters long'
                    },
                    regexp: {
                        regexp: /^[0-9a-zA-Z@._]+$/,
                        message: 'The contact can contain 0-9, a-z, A-z, #, (.), or (_) only'
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
    for (var i = 1 ; i < 3; i++) { audienceDetailRow.after(audienceDetailRow.clone()); }
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
            if (parseInt(visitType) == 1) { rowObj.find(".dwnBookStatus").parent().hide(); }
            else { rowObj.find(".dwnBookStatus").parent().show(); }
        }).change();
    });
    //$(".dwnFSMList").chosen({ width: "100%" });
    audiences.ValidateModalAudienceQuickForm($("#divAudienceBulkInsert"));
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
                        message: 'The first name must be more than 3 and less than 30 characters long'
                    },
                    regexp: {
                        regexp: /^['a-zA-Z0-9_ ]+$/,
                        message: 'The first name can containe a-z, A-Z, 0-9, \',( ), or (_) only'
                    }
                }
            },
            Contact: {
                message: 'The email address/contact no is not valid',
                validators: {
                    stringLength: {
                        min: 8,
                        max: 20,
                        message: 'The email address/contact no must be min 8 to 20 characters long'
                    },
                    notEmpty: {
                        message: 'The email address/contact no is required'
                    },
                    regexp: {
                        regexp: /^[0-9a-zA-Z@._]+$/,
                        message: 'The contact can contain 0-9, a-z, A-z, #, (.), or (_) only'
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
        var visitDate = formObj.find(".txtVisitDate").val();
        var visitTypeID = formObj.find("#dwnPeopleVistiType").val();
        var officeID = formObj.find("#dwnOffices").val();
        var eventID = formObj.find("#dwnEvetns").val();
        var conventionID = formObj.find("#dwnConvensions").val();
        var fsmName = formObj.find(".txtFSMName").val();
        //var fsmID = formObj.find("#dwnFSMList").val();
        var bookingStatus = formObj.find("#dwnBookStatus").val();
        var gsbAmount = formObj.find("#txtGSBAmount").val();
        var donationAmount = formObj.find("#txtDonationAmount").val();
        if (IsNullOrEmpty(officeID) && officeID <= 0) { officeID = 0; }
        if (IsNullOrEmpty(eventID) && eventID <= 0) { eventID = 0; }
        //if (IsNullOrEmpty(fsmID) && fsmID <= 0) { fsmID = 0; }
        if (IsNullOrEmpty(conventionID) && conventionID <= 0) { conventionID = 0; }
        if (IsNullOrEmpty(gsbAmount)) { gsbAmount = 0; }
        if (IsNullOrEmpty(donationAmount) && donationAmount <= 0) { donationAmount = 0; }
        if (visitTypeID == 1) { eventID = 0; conventionID = 0; }
        var dataObj = {
            audienceID: audienceID,
            name: name,
            visitDate: visitDate,
            contact: contact,
            visitType: visitTypeID,
            officeID: officeID,
            eventID: eventID,
            convensionID: conventionID,
            fsmName: fsmName,
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
                    ShowUpdateSuccessSaveAlert();
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
        //dialogContentPlaceHolder.find("#dwnFSMList").val(dialogContentPlaceHolder.find("#hdnFSMID").val());
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
                    window.location.reload();
                } else {
                }
            }
        });
    }, function (event, dataModalPlaceHolder) { });
};
audiences.UpdateAudienceAttendStatus = function (audienceID) {
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        async: false,
        data: JSON.stringify({ "id": audienceID }),
        url: audiences.options.UpdateAttendStatusURL,
        success: function (data) {
            audiences.LoadAudienceList();
        }
    });
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
            {
                "data": "Attended",
                "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                    var currentObj = $(cell);
                    currentObj.css({ "text-align": "center" }).data("task_detail", rowData);
                    currentObj.off("click.updateStatus").on("click.updateStatus", function () {
                        audiences.UpdateAudienceAttendStatus(rowData.ID);
                    });
                },
                render: function (o) {
                    return "<a href=\"#\">" + (o ? "<i class=\"icon ion-android-checkbox-outline\" style=\"font-size: 22px;\"></i>" : "<i class=\"icon ion-android-checkbox-outline-blank\" style=\"font-size: 22px;\"></i>") + "</a>";
                },
                "orderable": false,
                "width": '2%'
            },
            { "data": "Name" },
            { "data": "Contact" },
            { "data": "VisitDate" },
            { "data": "VisitType" },
            { "data": "EventName" },
            { "data": "ConventionName" },
            { "data": "FSMName" },
            { "data": "Status" },
            { "data": "GSBAmount" },
            { "data": "DonationAmount" },
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
            }]
    }).removeClass('display').addClass('table table-striped table-bordered');
};
audiences.UpdateGlobalTimePeriodSelection = function (start, end) {
    audiences.options.startDate = start.toDate();
    audiences.options.endDate = end.toDate();
    $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
};
audiences.LoadGlobalTimeFilter = function () {
    $('#reportrange').daterangepicker({
        "startDate": moment().startOf('week').isoWeekday(4),
        "endDate": moment().endOf('week').isoWeekday(4),
        ranges: {
            'Last 7 Days': [moment().startOf('week').isoWeekday(4), moment().endOf('week').isoWeekday(4)],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, audiences.UpdateGlobalTimePeriodSelection).off("apply.daterangepicker").on('apply.daterangepicker', function (ev, picker) {
        audiences.LoadAudienceList();
    });
    audiences.UpdateGlobalTimePeriodSelection(moment().startOf('week').isoWeekday(4), moment().endOf('week').isoWeekday(4));
    audiences.LoadAudienceList();
};
audiences.DoPageSetting = function () {
    audiences.LoadGlobalTimeFilter();
    audiences.LoadQuickBooking();
};

