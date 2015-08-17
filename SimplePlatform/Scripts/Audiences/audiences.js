var audiences = {};
audiences.options = {
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
        alert(1);
        //var formObj = $(e.target);;
        //var firstName = formObj.find("#txtUserFirstName").val();
        //var lastName = formObj.find("#txtUserLastName").val();
        //var emailID = formObj.find("#txtUserEmailAddress").val();
        //var visitTypeID = formObj.find("#dwnPeopleVistiType").val();
        //var officeID = formObj.find("#dwnOffices").val();
        //var eventID = formObj.find("#dwnEvetns").val();
        //var fsmID = formObj.find("#dwnFSMList").val();
        //var convensionID = formObj.find("#dwnConvensions").val();
        //if (IsNullOrEmpty(officeID) && officeID <= 0) { officeID = 0; }
        //if (IsNullOrEmpty(eventID) && eventID <= 0) { eventID = 0; }
        //if (IsNullOrEmpty(fsmID) && fsmID <= 0) { fsmID = 0; }
        //if (IsNullOrEmpty(convensionID) && convensionID <= 0) { convensionID = 0; }
        //$.ajax({
        //    dataType: "json",
        //    contentType: "application/json; charset=utf-8",
        //    type: "POST",
        //    url: "/Audiences/Add",
        //    async: false,
        //    data: JSON.stringify({ "firstName": firstName, "lastName": lastName, "emailID": emailID, "visitTypeID": visitTypeID, "officeID": officeID, "eventID": eventID, "fsmID": fsmID, "convensionID": convensionID }),
        //    success: function (data) {
        //        var status = data;
        //        if (status) {
        //            obj.modal('hide');
        //        } else {
        //        }
        //    }
        //});
    });
};
audiences.LoadQuickBooking = function () {
    var audienceDetailRow = $(".audienceDetailRow");
    for (var i = 1 ; i < 3; i++) { audienceDetailRow.after(audienceDetailRow.clone()); }
    $(".btnAddUsers").off("click.btnAddUsers").on("click.btnAddUsers", $.proxy(function () { }, this));
    $(".txtVisitDate").datepicker({ autoclose: true, todayHighlight: true });
    $(".dwnVisitType").chosen({ width: "100%" });
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
            { "data": "EmailID" },
            { "data": "VisitType" },
            { "data": "ConventionEventName" },
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