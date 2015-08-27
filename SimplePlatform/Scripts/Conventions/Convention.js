var conventions = {};
conventions.options = {
    EditViewURL: "/Conventions/Edit/",
    UpdateURL: "/Conventions/Update",
    DeleteURL: "/Conventions/Delete"
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
                            regexp: /^[a-zA-Z0-9_]+$/,
                            message: 'The name can contain a-z, A-Z, 0-9, or (_) only'
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
$(document).ready(function () {
    $('#myDataTable').dataTable({
        renderer: {
            "header": "bootstrap",
            "pageButton": "bootstrap"
        },
        "ajax": {
            "url": "/Conventions/GetConventions",
            "type": "POST"
        },
        "displayLength": 25,
        responsive: true,
        "deferRender": true,
        "columns": [{ "data": "name" }, { "data": "startDate" }, { "data": "endDate" }, {
            "data": "description"
        }, {
            "data": null,
            "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                var currentObj = $(cell);
                currentObj.css({ "text-align": "center" }).data("convention_detail", rowData);
                currentObj.off("click.dataTableEditLink").on("click.dataTableEditLink", function () { conventions.EditConventionDetail($(this)); });
            },
            render: function (o) { return '<a href="#"><i class="ui-tooltip fa fa-pencil" style="font-size: 22px;" data-original-title="Edit"></i></a>'; },
            "orderable": false,
            "width": '2%'
        }, {
            "data": null,
            "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                var currentObj = $(cell);
                currentObj.css({ "text-align": "center" }).data("convention_detail", rowData);
                currentObj.off("click.dataTableDeleteLink").on("click.dataTableDeleteLink", function () { conventions.DeletConventionDetail($(this)); });
            },
            render: function (o) { return '<a href="#"><i class="ui-tooltip fa fa-trash-o" style="font-size: 22px;" data-original-title="Delete"></i></a>'; },
            "orderable": false,
            "width": '2%'
        }]
    }).removeClass('display').addClass('table table-striped table-bordered');
});