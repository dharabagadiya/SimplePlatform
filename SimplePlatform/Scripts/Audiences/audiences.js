var audiences = {};
audiences.options = {
    AddDataURL: "/Audiences/Add"
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
        var fsmID = parseInt(formObj.find(".dwnFSMList").val());
        var placeID = parseInt(formObj.find("#hdnVisitPlaceID").val());
        var officeID = 0;
        var eventID = 0;
        var convensionID = 0;
        var donationAmount = parseFloat(formObj.find(".txtDonationAmount").val());
        var bookingStatus = parseFloat(formObj.find(".dwnBookStatus").val());
        if (visitType == 1) { officeID = placeID; }
        else if (visitType == 2) { eventID = placeID; }
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
                fsmID: fsmID,
                bookingStatus: bookingStatus,
                donationAmount: donationAmount
            };
            ajaxSubmit.push(audiences.AddAudienceAjaxCall(dataObj, formObj));
        }
    });
    $.when(ajaxSubmit).done(function () { window.location.reload(); });
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
                message: 'The first name is not valid',
                validators: {
                    stringLength: {
                        min: 3,
                        max: 15,
                        message: 'The first name must be more than 3 and less than 15 characters long'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_]+$/,
                        message: 'The first name can containe a-z, A-Z, 0-9, or (_) only'
                    }
                }
            },
            Contact: {
                message: 'The Contact No is not valid',
                validators: {
                    stringLength: {
                        min: 10,
                        max: 10,
                        message: 'The Contact must be 10 characters long'
                    },
                    regexp: {
                        regexp: /^[1-9][0-9]{0,15}$/,
                        message: 'The Contact can contain 0-9 only'
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
    $(".dwnVisitType").each(function () {
        $(this).chosen({ width: "100%" }).off("change").on("change", function () {
            var rowObj = $(this).closest(".audienceDetailRow");
            var groupObj = $(this.options[this.selectedIndex]).closest('optgroup');
            var visitType = 0, placeID = 0;
            if (groupObj.length > 0) { visitType = groupObj.attr("id"); placeID = $(this).val(); } else { placeID = $(this).val(); }
            rowObj.find("#hdnVisitType").val(visitType);
            rowObj.find("#hdnVisitPlaceID").val(placeID);
        }).change();
    });
    $(".dwnFSMList").chosen({ width: "100%" });
    $(".dwnBookStatus").chosen({ width: "100%" });
    audiences.ValidateModalAudienceForm($("#divAudienceBulkInsert"));
};
audiences.LoadAudienceList = function () {
    $('#audienceList').dataTable({
        renderer: {
            "header": "bootstrap",
            "pageButton": "bootstrap"
        },
        "ajax": {
            "url": "/Audiences/GetAudiences",
            "type": "POST"
        },
        "displayLength": 25,
        responsive: true,
        "deferRender": true,
        "columns": [
            { "data": "Name" },
            { "data": "Contact" },
            { "data": "VisitDate" },
            { "data": "VisitType" },
            { "data": "EventName" },
            { "data": "ConventionName" },
            { "data": "Status", "width": '10%' },
            {
                "data": null,
                "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                    var currentObj = $(cell);
                    currentObj.css({ "text-align": "center" }).data("event_detail", rowData);
                    currentObj.off("click.dataTableEditLink").on("click.dataTableEditLink", function () { events.EditEventDetail($(this)); });
                },
                render: function (o) { return '<a href="#"><i class="ui-tooltip fa fa-pencil" style="font-size: 22px;" data-original-title="Edit"></i></a>'; },
                "orderable": false,
                "width": '2%'
            }, {
                "data": null,
                "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                    var currentObj = $(cell);
                    currentObj.css({ "text-align": "center" }).data("event_detail", rowData);
                    currentObj.off("click.dataTableDeleteLink").on("click.dataTableDeleteLink", function () { events.DeletEventDetail($(this)); });
                },
                render: function (o) { return '<a href="#"><i class="ui-tooltip fa fa-trash-o" style="font-size: 22px;" data-original-title="Delete"></i></a>'; },
                "orderable": false,
                "width": '2%'
            }]
    }).removeClass('display').addClass('table table-striped table-bordered');
};
$(document).ready(function () {
    audiences.LoadAudienceList();
    audiences.LoadQuickBooking();
});