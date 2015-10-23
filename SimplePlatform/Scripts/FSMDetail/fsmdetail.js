var fsmdetail = {};
fsmdetail.options = {
    GetFSMUserURL: "/FSMDetail/GetFSMUsers",
    EditDataURL: function (id) { return ("/FSMDetail/Edit/" + id); },
    UpdateDataURL: "/FSMDetail/Update",
    DeleteDataURL: "/FSMDetail/Delete"
};
fsmdetail.DeleteFSMDetail = function (obj) {
    var currentObj = obj;
    var fsmDetail = obj.data("fsmuser_detail");
    ShowOkCancelDialogBox($("#divCommonModalPlaceHolder"), "Delete", "Are you sure you want to delete record?", function (event, dataModalPlaceHolder) {
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            url: fsmdetail.options.DeleteDataURL,
            async: false,
            data: JSON.stringify({ "id": fsmDetail.ID }),
            success: function (data) {
                var status = data;
                if (status) {
                    fsmdetail.LoadFSMUserList();
                } else {
                }
            }
        });
    }, function (event, dataModalPlaceHolder) { });
};
fsmdetail.ValidateModalUpdateFSMDetailForm = function (obj) {
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
        var formObj = $(e.target);
        var id = formObj.find("#hdnFSMUserDetail").val();
        var name = formObj.find("#txtUserName").val();
        var emailID = formObj.find("#txtUserEmailAddress").val();
        var phoneNumber = formObj.find("#txtPhoneNumber").val();
        var dataObj = {
            id: id,
            name: name,
            emailID: emailID,
            phoneNumber: phoneNumber
        };
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            url: fsmdetail.options.UpdateDataURL,
            data: JSON.stringify(dataObj),
            success: function (data) {
                var status = data;
                if (status != 0) {
                    obj.modal('hide');
                    fsmdetail.LoadFSMUserList();
                } else {
                    dialogContentPlaceHolder.find("#divCommonMessage").removeClass("hidden");
                }
            }
        });
    });
};
fsmdetail.EditFSMDetail = function (obj) {
    var fsmDetail = obj.data("fsmuser_detail");
    $("#divCommonModalPlaceHolder").empty();
    ShowDialogBox($("#divCommonModalPlaceHolder"), fsmdetail.options.EditDataURL(fsmDetail.ID), null, $.proxy(function (event, dialogContentPlaceHolder) {
        dialogContentPlaceHolder.find("#divCommonMessage").addClass("hidden");
        fsmdetail.ValidateModalUpdateFSMDetailForm(dialogContentPlaceHolder);
    }, this));
};
fsmdetail.LoadFSMUserList = function () {
    $('#myDataTable').dataTable().fnDestroy();
    $('#myDataTable').dataTable({
        renderer: {
            "header": "bootstrap",
            "pageButton": "bootstrap"
        },
        "ajax": {
            "url": fsmdetail.options.GetFSMUserURL,
            "type": "POST"
        },
        "displayLength": 25,
        responsive: true,
        "deferRender": true,
        "columns": [
            { "data": "Name" },
            { "data": "EmailID" },
            { "data": "PhoneNumber" },
            {
                "data": null,
                "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                    var currentObj = $(cell);
                    currentObj.css({ "text-align": "center" }).data("fsmuser_detail", rowData);
                    currentObj.off("click.dataTableEditLink").on("click.dataTableEditLink", function () { fsmdetail.EditFSMDetail($(this)); });
                },
                render: function (o) { return '<a href="#"><i class="ui-tooltip fa fa-pencil" style="font-size: 22px;" data-original-title="Edit"></i></a>'; },
                "orderable": false,
                "width": '2%'
            }, {
                "data": null,
                "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                    var currentObj = $(cell);
                    currentObj.css({ "text-align": "center" }).data("fsmuser_detail", rowData);
                    currentObj.off("click.dataTableDeleteLink").on("click.dataTableDeleteLink", function () { fsmdetail.DeleteFSMDetail($(this)); });
                },
                render: function (o) { return '<a href="#"><i class="ui-tooltip fa fa-trash-o" style="font-size: 22px;" data-original-title="Delete"></i></a>'; },
                "orderable": false,
                "width": '2%'
            }
        ]
    }).removeClass('display').addClass('table table-striped table-bordered');
};
fsmdetail.DoPageSetting = function () {
    fsmdetail.LoadFSMUserList();
};