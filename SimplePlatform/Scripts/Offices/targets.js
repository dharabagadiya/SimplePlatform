var targets = {};
targets.options = {
    AddDataURL: "/Targets/Add",
    EditDataURL: function (id) { return ("/Targets/Edit/" + id); },
    UpdateDataURL: "/Targets/Update"
};
targets.AddTargetsAjaxCall = function (obj, containerObj) {
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        url: targets.options.AddDataURL,
        async: false,
        data: JSON.stringify(obj),
        success: function (data) {
            containerObj.find(".dwnOffices").val(0);
            containerObj.find(".txtBookingTarget").val(0);
            containerObj.find(".txtDonationAmount").val(0);
            containerObj.find(".txtGSBAmountTarget").val(0);
            containerObj.find(".txtArrivalTargets").val(0);
        }
    });
};
targets.SubmitBulkInsertForm = function (obj) {
    var ajaxSubmit = [];
    obj.find(".divTargetBulkInsertRow").each(function () {
        var formObj = $(this);
        var officeID = formObj.find(".dwnOffices").val();
        var dueDate = formObj.find(".txtDueDate").val();
        var bookingTargets = formObj.find(".txtBookingTarget").val();
        var fundRaisingAmount = formObj.find(".txtDonationAmount").val();
        var gsbAmount = formObj.find(".txtGSBAmountTarget").val();
        var arrivalTargets = formObj.find(".txtArrivalTargets").val();
        if (IsNullOrEmpty(bookingTargets)) { bookingTargets = 0; }
        if (IsNullOrEmpty(fundRaisingAmount)) { fundRaisingAmount = 0; }
        if (IsNullOrEmpty(gsbAmount)) { gsbAmount = 0; }
        if (IsNullOrEmpty(arrivalTargets)) { arrivalTargets = 0; }
        if (officeID != 0 && fundRaisingAmount != 0) {
            var dataObj = {
                officeID: officeID,
                dueDate: dueDate,
                bookingTargets: parseInt(bookingTargets),
                fundRaisingAmount: parseFloat(fundRaisingAmount),
                gsbAmount: parseFloat(gsbAmount),
                arrivalTargets: parseInt(arrivalTargets)
            };
            ajaxSubmit.push(targets.AddTargetsAjaxCall(dataObj, formObj));
        }
    });
    $.when(ajaxSubmit).done(function () { window.location.reload(); });
};
targets.ValidateModalTargetForm = function (obj) {
    obj.find("form")
    .bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            Amount: {
                validators: {
                    numeric: {
                        message: 'The value is not a number',
                        thousandsSeparator: '',
                        decimalSeparator: '.'
                    }
                }
            }
        }
    }).off('success.form.bv').on('success.form.bv', function (e) {
        e.preventDefault();
        targets.SubmitBulkInsertForm(obj);
    });
};
targets.LoadQuickTargetSetting = function () {
    var tagetsRowDetails = $(".divTargetBulkInsertRow");
    for (var i = 1 ; i < 4; i++) { tagetsRowDetails.after(tagetsRowDetails.clone()); }
    $(".txtDueDate").datepicker({ autoclose: true, todayHighlight: true });
    targets.ValidateModalTargetForm($("#divTargetBulkInsert"));
};

targets.ValidateModalEditTargetForm = function (obj) {
    obj.find("form")
    .bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            Amount: {
                validators: {
                    numeric: {
                        message: 'The value is not a number',
                        thousandsSeparator: '',
                        decimalSeparator: '.'
                    }
                }
            }
        }
    }).off('success.form.bv').on('success.form.bv', function (e) {
        e.preventDefault();
        var formObj = $(this);
        var targetID = formObj.find("#hdnTargetID").val();
        var officeID = formObj.find(".dwnOffices").val();
        var dueDate = formObj.find(".txtDueDate").val();
        var bookingTargets = formObj.find(".txtBookingTarget").val();
        var fundRaisingAmount = formObj.find(".txtDonationAmount").val();
        var gsbAmount = formObj.find(".txtGSBAmountTarget").val();
        var arrivalTargets = formObj.find(".txtArrivalTargets").val();
        if (IsNullOrEmpty(bookingTargets)) { bookingTargets = 0; }
        if (IsNullOrEmpty(fundRaisingAmount)) { fundRaisingAmount = 0; }
        if (IsNullOrEmpty(gsbAmount)) { gsbAmount = 0; }
        if (IsNullOrEmpty(arrivalTargets)) { arrivalTargets = 0; }
        var dataObj = {
            targetID: targetID,
            officeID: officeID,
            dueDate: dueDate,
            bookingTargets: parseInt(bookingTargets),
            fundRaisingAmount: parseFloat(fundRaisingAmount),
            gsbAmount: parseFloat(gsbAmount),
            arrivalTargets: parseInt(arrivalTargets)
        };
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            url: targets.options.UpdateDataURL,
            async: false,
            data: JSON.stringify(dataObj),
            success: function (data) {
                var status = data;
                if (status) { window.location.reload(); } else { }
            }
        });
    });
};
targets.EditTargetDetail = function (obj) {
    var targetDetail = obj.data("target_detail");
    $("#divCommonModalPlaceHolder").empty();
    ShowDialogBox($("#divCommonModalPlaceHolder"), targets.options.EditDataURL(targetDetail.ID), null, $.proxy(function (event, dialogContentPlaceHolder) {
        dialogContentPlaceHolder.find(".txtDueDate").datepicker({ autoclose: true, todayHighlight: true });
        dialogContentPlaceHolder.find("#dwnOffices").val(dialogContentPlaceHolder.find("#hdnOfficesID").val()).change();
        targets.ValidateModalEditTargetForm(dialogContentPlaceHolder);
    }, this));
};
targets.LoadTagetsList = function () {
    $('#targetList').dataTable({
        renderer: {
            "header": "bootstrap",
            "pageButton": "bootstrap"
        },
        "ajax": {
            "url": "/Targets/GetTasks",
            "type": "POST"
        },
        "displayLength": 25,
        responsive: true,
        "deferRender": true,
        "columns": [
            { "data": "OfficeName" },
            { "data": "DueDate" },
            { "data": "Booking" },
            { "data": "FundRaising" },
            { "data": "GSB" },
            { "data": "Arrivals" },
            {
                "data": null,
                "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                    var currentObj = $(cell);
                    currentObj.css({ "text-align": "center" }).data("target_detail", rowData);
                    currentObj.off("click.dataTableEditLink").on("click.dataTableEditLink", function () { targets.EditTargetDetail($(this)); });
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
$(document).ready(function () {
    targets.LoadTagetsList();
    targets.LoadQuickTargetSetting();
});