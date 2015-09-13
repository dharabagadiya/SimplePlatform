var events = {};
events.options = {
    EditViewURL: "/Events/Edit/",
    UpdateURL: "/Events/Update",
    DeleteURL: "/Events/Delete",
    DetailPageURL: function (id) { return ("/Events/Detail/" + id); },
    startDate: null,
    endDate: null
};
events.BindEventRowClickEvent = function (obj) {
    obj.DataTable().off("select.dt").on("select.dt", function (e, dt, type, indexes) {
        if (type === 'row') {
            var dataObj = obj.DataTable().rows(indexes).data();
            var eventID = dataObj.pluck("id")[0];
            window.location.href = events.options.DetailPageURL(eventID);
        }
    });
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
                        ShowUpdateSuccessSaveAlert();
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
        dialogContentPlaceHolder.find("#datetimerange").daterangepicker({ timePicker24Hour: true, timePicker: true, timePickerIncrement: 15, locale: { format: 'MM/DD/YYYY HH:mm' } });
        dialogContentPlaceHolder.find("#divCommonMessage").addClass("hidden");
    }, this));
};
events.DeletEventDetail = function (obj) {
    var currentObj = obj;
    var EventDetail = obj.data("event_detail");
    ShowOkCancelDialogBox($("#divCommonModalPlaceHolder"), "Delete", "Are you sure you want to delete record?", function (event, dataModalPlaceHolder) {
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
    }, function (event, dataModalPlaceHolder) { });
};
events.LoadEventsGrid = function () {
    $('#myDataTable').dataTable().fnDestroy();
    $('#myDataTable').dataTable({
        "select": "single",
        renderer: {
            "header": "bootstrap",
            "pageButton": "bootstrap"
        },
        "ajax": {
            "url": "/Events/GetEvents",
            "type": "POST",
            "data": { startDate: events.options.startDate.toDateString(), endDate: events.options.endDate.toDateString() }
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
                render: function (o) {
                    if (!o.IsUpdateEnable) { return "-"; }
                    return '<a href="#"><i class="ui-tooltip fa fa-pencil" style="font-size: 22px;" data-original-title="Edit"></i></a>';
                },
                "orderable": false,
                "width": '2%'
            }, {
                "data": null,
                "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                    var currentObj = $(cell);
                    currentObj.css({ "text-align": "center" }).data("event_detail", rowData);
                    currentObj.off("click.dataTableDeleteLink").on("click.dataTableDeleteLink", function () { events.DeletEventDetail($(this)); });
                },
                render: function (o) {
                    if (!o.IsUpdateEnable) { return "-"; }
                    return '<a href="#"><i class="ui-tooltip fa fa-trash-o" style="font-size: 22px;" data-original-title="Delete"></i></a>';
                },
                "orderable": false,
                "width": '2%'
            }]
    }).removeClass('display').addClass('table table-striped table-bordered');
    events.BindEventRowClickEvent($('#myDataTable'));
};
events.UpdateGlobalTimePeriodSelection = function (start, end) {
    events.options.startDate = start.toDate();
    events.options.endDate = end.toDate();
    $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
};
events.LoadGlobalTimeFilter = function () {
    $('#reportrange').daterangepicker({
        "startDate": moment().startOf('week').subtract(2, 'days'),
        "endDate": moment().startOf('week').add('days', 4),
        ranges: {
            'Last 7 Days': [moment().startOf('week').subtract(2, 'days'), moment().startOf('week').add('days', 4)],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, events.UpdateGlobalTimePeriodSelection).off("apply.daterangepicker").on('apply.daterangepicker', function (ev, picker) {
        events.LoadEventsGrid();
    });
    events.UpdateGlobalTimePeriodSelection(moment().startOf('week').subtract(2, 'days'), moment().startOf('week').add('days', 4));
    events.LoadEventsGrid();
};
events.DoPageSetting = function () { events.LoadGlobalTimeFilter(); };
