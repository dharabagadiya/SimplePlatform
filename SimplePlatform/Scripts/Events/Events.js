var events = {};
events.options = {
    EditViewURL: "/Events/Edit/",
    UpdateURL: "/Events/Update",
    DeleteURL: "/Events/Delete"
};
events.ValidateModalEventForm = function (obj) {
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
                }
            }
        }).on('success.form.bv', function (e) {
            e.preventDefault();
            var formObj = $(e.target);;
            var name = formObj.find("#txtName").val();
            var startDates = formObj.find("#txtDueDateStart").val() + " " + formObj.find("#timepickerStart").val();
            var endDates = formObj.find("#txtDueDateEnd").val() + " " + formObj.find("#timepickerEnd").val();
            var description = formObj.find("#txtDescription").val();
            var eventID = formObj.find("#hdnEventID").val();
            var officeID = formObj.find("#dwnOffices").val();
            var conventionID = formObj.find("#dwnConvention").val();
            var city = formObj.find("#txtCity").val();
            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                url: events.options.UpdateURL,
                async: false,
                data: JSON.stringify({ "name": name, "startDate": startDates, "endDate": endDates, "description": description, "officeID": officeID, "eventID": eventID, "conventionID": conventionID, "city": city }),
                success: function (data) {
                    var status = data;
                    if (status) {
                        obj.modal('hide');
                        $('#myDataTable').dataTable().api().ajax.reload(null, false);
                    } else {
                        obj.find("#divCommonMessage").removeClass("hidden");
                    }
                }
            });
        });
};
events.EditEventDetail = function (obj) {
    var currentObj = obj;
    var EventDetail = obj.data("event_detail");
    $("#divCommonModalPlaceHolder").empty();
    ShowDialogBox($("#divCommonModalPlaceHolder"), (events.options.EditViewURL + EventDetail.id), null, $.proxy(function (event, dialogContentPlaceHolder) {
        this.ValidateModalEventForm(dialogContentPlaceHolder);
        $("#dwnOffices").val($("#hdnOfficeID").val());
        $("#dwnConvention").val($("#hdnConventionID").val());
        dialogContentPlaceHolder.find('#datepickerStart').datepicker({ autoclose: true, todayHighlight: true });
        dialogContentPlaceHolder.find('#datepickerEnd').datepicker({ autoclose: true, todayHighlight: true });
        dialogContentPlaceHolder.find('#timepickerStart').timepicker({ showMeridian: false, upArrowStyle: 'fa fa-angle-up', downArrowStyle: 'fa fa-angle-down', });
        dialogContentPlaceHolder.find('#timepickerEnd').timepicker({ showMeridian: false, upArrowStyle: 'fa fa-angle-up', downArrowStyle: 'fa fa-angle-down', });
        dialogContentPlaceHolder.find("#divCommonMessage").addClass("hidden");
    }, this));
};
events.DeletEventDetail = function (obj) {
    var currentObj = obj;
    var EventDetail = obj.data("event_detail");
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        url: events.options.DeleteURL,
        async: false,
        data: JSON.stringify({ "id": EventDetail.id }),
        success: function (data) {
            var status = data;
            if (status) {
                $('#myDataTable').dataTable().api().ajax.reload(null, false);
            } else {
            }
        }
    });
};
$(document).ready(function () {
    $('#myDataTable').dataTable({
        renderer: {
            "header": "bootstrap",
            "pageButton": "bootstrap"
        },
        "ajax": {
            "url": "/Events/GetEvents",
            "type": "POST"
        },
        "displayLength": 25,
        responsive: true,
        "deferRender": true,
        "columns": [{ "data": "name" }, { "data": "startDate" }, { "data": "endDate" }, { "data": "description" }, { "data": "city" },
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
});