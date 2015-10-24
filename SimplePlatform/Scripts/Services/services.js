var services = {};
services.options = {
    GetServicesList: "/Services/GetServices",
    EditDataURL: function (id) { return ("/Services/Edit/" + id); },
    UpdateServiceURL: "/Services/Update",
    DeleteServiceURL: "/Services/Delete"
};

services.DeleteServiceType = function (obj) {
    var currentObj = obj;
    var service = obj.data("service_detail");
    ShowOkCancelDialogBox($("#divCommonModalPlaceHolder"), "Delete", "Are you sure you want to delete record?", function (event, dataModalPlaceHolder) {
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            url: services.options.DeleteServiceURL,
            async: false,
            data: JSON.stringify({ id: service.ServiceId }),
            success: function (data) {
                var status = data;
                if (status) {
                    services.LoadServicesList();
                } else {
                }
            }
        });
    }, function (event, dataModalPlaceHolder) { });
};

services.ValidateModalUpdateServiceModelForm = function (obj) {
    obj.find("form").bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            txtServiceName: {
                message: 'The service name is not valid',
                validators: {
                    notEmpty: {
                        message: 'The service name is required and cannot be empty'
                    },
                    stringLength: {
                        min: 3,
                        max: 30,
                        message: 'The service name must be more than 3 and less than 30 characters long'
                    },
                    regexp: {
                        regexp: /^['a-zA-Z0-9_ ]+$/,
                        message: 'The service name can containe a-z, A-Z, 0-9, \',( ), or (_) only'
                    }
                }
            }
        }
    }).off('success.form.bv').on('success.form.bv', function (e) {
        e.preventDefault();
        var formObj = $(e.target);
        var id = formObj.find("#hdnServiceID").val();
        var name = formObj.find("#txtServiceName").val();
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            url: services.options.UpdateServiceURL,
            data: JSON.stringify({ id: id, name: name }),
            success: function (data) {
                var status = data;
                if (status != 0) {
                    obj.modal('hide');
                    services.LoadServicesList();
                } else {
                    dialogContentPlaceHolder.find("#divCommonMessage").removeClass("hidden");
                }
            }
        });
    });
};
services.EditServiceDetail = function (obj) {
    var service = obj.data("service_detail");
    $("#divCommonModalPlaceHolder").empty();
    ShowDialogBox($("#divCommonModalPlaceHolder"), services.options.EditDataURL(service.ServiceId), null, $.proxy(function (event, dialogContentPlaceHolder) {
        dialogContentPlaceHolder.find("#divCommonMessage").addClass("hidden");
        services.ValidateModalUpdateServiceModelForm(dialogContentPlaceHolder);
    }, this));
};

services.LoadServicesList = function () {
    $('#myDataTable').dataTable().fnDestroy();
    $('#myDataTable').dataTable({
        renderer: {
            "header": "bootstrap",
            "pageButton": "bootstrap"
        },
        "ajax": {
            "url": services.options.GetServicesList,
            "type": "POST"
        },
        "displayLength": 25,
        responsive: true,
        "deferRender": true,
        "columns": [
            { "data": "ServiceName" },
            {
                "data": null,
                "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                    var currentObj = $(cell);
                    currentObj.css({ "text-align": "center" }).data("service_detail", rowData);
                    currentObj.off("click.dataTableEditLink").on("click.dataTableEditLink", function () { services.EditServiceDetail($(this)); });
                },
                render: function (o) { return '<a href="#"><i class="ui-tooltip fa fa-pencil" style="font-size: 22px;" data-original-title="Edit"></i></a>'; },
                "orderable": false,
                "width": '2%'
            }, {
                "data": null,
                "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                    var currentObj = $(cell);
                    currentObj.css({ "text-align": "center" }).data("service_detail", rowData);
                    currentObj.off("click.dataTableDeleteLink").on("click.dataTableDeleteLink", function () { services.DeleteServiceType($(this)); });
                },
                render: function (o) { return '<a href="#"><i class="ui-tooltip fa fa-trash-o" style="font-size: 22px;" data-original-title="Delete"></i></a>'; },
                "orderable": false,
                "width": '2%'
            }
        ]
    }).removeClass('display').addClass('table table-striped table-bordered');
};
services.DoPageSetting = function () {
    services.LoadServicesList();
};
