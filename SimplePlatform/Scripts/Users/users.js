var users = {};
users.options = {
    EditViewURL: "/Users/Edit/",
    UpdateURL: "/Users/Update",
    DeleteURL: "/Users/Delete"
};
users.ValidateModalUserForm = function (obj) {
    obj.find("form")
    .bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            firstName: {
                message: 'The first name is not valid',
                validators: {
                    notEmpty: {
                        message: 'The first name cannot be empty'
                    },
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
            lastName: {
                message: 'The last name is not valid',
                validators: {
                    notEmpty: {
                        message: 'The last name is required and cannot be empty'
                    },
                    stringLength: {
                        min: 3,
                        max: 15,
                        message: 'The last name must be more than 3 and less than 15 characters long'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_]+$/,
                        message: 'The last name can containe a-z, A-Z, 0-9, or (_) only'
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
            }
        }
    }).on('success.form.bv', function (e) {
        e.preventDefault();
        var formObj = $(e.target);
        var id = formObj.find("#txtUserID").val();
        var firstName = formObj.find("#txtUserFirstName").val();
        var lastName = formObj.find("#txtUserLastName").val();
        var emailID = formObj.find("#txtUserEmailAddress").val();
        var userRoleID = formObj.find("#dwnUserRoles").val();
        var officesID = formObj.find("#dwnOfficeList").length > 0 ? formObj.find("#dwnOfficeList").val() : 0;
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            url: users.options.UpdateURL,
            async: false,
            data: JSON.stringify({ "id": id, "firstName": firstName, "lastName": lastName, "emildID": emailID, "userRoleID": userRoleID, "officesID": officesID }),
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
users.EditUserDetail = function (obj) {
    var currentObj = obj;
    var userDetail = obj.data("user_detail");
    $("#divCommonModalPlaceHolder").empty();
    ShowDialogBox($("#divCommonModalPlaceHolder"), (users.options.EditViewURL + userDetail.id), null, $.proxy(function (event, dialogContentPlaceHolder) {
        this.ValidateModalUserForm(dialogContentPlaceHolder);
        dialogContentPlaceHolder.find("#divCommonMessage").addClass("hidden");
        dialogContentPlaceHolder.find("#dwnUserRoles").val(userDetail.userRolesID);
        dialogContentPlaceHolder.find("#dwnOfficeList").val(userDetail.userOfficesID);
    }, this));
};
users.DeletUserDetail = function (obj) {
    var currentObj = obj;
    var userDetail = obj.data("user_detail");
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        url: users.options.DeleteURL,
        async: false,
        data: JSON.stringify({ "id": userDetail.id }),
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
            "url": "/Users/GetUsers",
            "type": "POST"
        },
        "displayLength": 25,
        responsive: true,
        "deferRender": true,
        "columns": [{ "data": "firstName" }, { "data": "lastName" }, { "data": "userRoles" }, {
            "data": "createDate"
        }, {
            "data": null,
            "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                var currentObj = $(cell);
                currentObj.css({ "text-align": "center" }).data("user_detail", rowData);
                currentObj.off("click.dataTableEditLink").on("click.dataTableEditLink", function () { users.EditUserDetail($(this)); });
            },
            render: function (o) { return '<a href="#"><i class="ui-tooltip fa fa-pencil" style="font-size: 22px;" data-original-title="Edit"></i></a>'; },
            "orderable": false,
            "width": '2%'
        }, {
            "data": null,
            "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                var currentObj = $(cell);
                currentObj.css({ "text-align": "center" }).data("user_detail", rowData);
                currentObj.off("click.dataTableDeleteLink").on("click.dataTableDeleteLink", function () { users.DeletUserDetail($(this)); });
            },
            render: function (o) { return '<a href="#"><i class="ui-tooltip fa fa-trash-o" style="font-size: 22px;" data-original-title="Delete"></i></a>'; },
            "orderable": false,
            "width": '2%'
        }]
    }).removeClass('display').addClass('table table-striped table-bordered');
});